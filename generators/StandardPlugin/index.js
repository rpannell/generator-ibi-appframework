'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
var globalfs = require('fs');
var mkdirp = require('mkdirp');
const ibigenerator = require('../../ibigenerator');
const moment = require('moment');
module.exports = class extends Generator {
	constructor(args, opts) {
		super(args, opts);
		
		this.option('pluginName', { description: "The Name of the plugin", type: String, alias: "pn" });
		this.option('sourceLocation', { description: "The location of the source code", type: String, alias: "sl" });
		this.option('createMasterSolution', { description: "Tells the solution to create the master solution with a service with the same name as the plugin", type: Boolean, default: false, alias: "ms" });
		
		this.option('webServiceUrl', { description: "The URL of the development web service", type: String, default: false, alias: "wsurl" });
		this.option('webServiceTestUrl', { description: "The URL of the development web service", type: String, default: false, alias: "wsurl" });
		this.option('webServiceProdUrl', { description: "The URL of the development web service", type: String, default: false, alias: "wsurl" });
		ibigenerator.resetFilesAndFolders();
	}
	prompting() {
		const prompts = [];
		if(this.options.pluginName == undefined){
			prompts.push({
				type: 'input',
				name: 'pluginName',
				message: 'What is the name of the plugin?',
				validate: function(input){
					return input == null || input == undefined || input == "" ? "Plugin Name is required" : true;
				}
			});
		}
		if(this.options.sourceLocation == undefined){
			prompts.push({
				type: 'input',
				name: 'sourceCodeLocation',
				message: 'Where is your source code located',
				default: 'C:\\Code\\Dev\\',
				validate: function(input){
					return input == null || input == undefined || input == "" ? "Source Code location is required" : true;
				},
				store: true
			});
		}
		
		return this.prompt(prompts).then(props => {
		  this.props = props;
		});
	}
	_pluginSourceLocation(){
		return path.join(this.templatedata.SourceLoc, 'Plugins', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Plugin');
	}
	_runTemplateOnFolder(folderName, sourceLocation){
		this.fs.copyTpl(
		  this.templatePath('IBI.PluginTemplate.Plugin/' + folderName + '/**/*'),
		  path.join(sourceLocation, folderName),
		  this.templatedata
		);
	}
	_createDirectory(directory){
		if(!globalfs.existsSync(directory)){ mkdirp.sync(directory); }
	}
	_buildTemplateData() {
		this.templatedata = {};
		this.templatedata.Version = ibigenerator.currentVersion();
		this.templatedata.TodaysDate = moment().format("YYYY-MM-DD, hh:mm A");
		this.templatedata.Name = this.options.pluginName == undefined || this.options.pluginName == null ? this.props.pluginName : this.options.pluginName;
		this.templatedata.SourceLoc = this.options.sourceLocation == undefined || this.options.sourceLocation == null ? this.props.sourceCodeLocation : this.options.sourceLocation;
		this.templatedata.WebServiceUrl = this.options.webServiceUrl == undefined || this.options.webServiceUrl == null ? this.props.webServiceUrl : this.options.webServiceUrl;
		this.templatedata.WebServiceTestUrl = this.options.webServiceTestUrl == undefined || this.options.webServiceTestUrl == null ? this.props.webServiceTestUrl : this.options.webServiceTestUrl;
		this.templatedata.WebServiceProdUrl = this.options.webServiceProdUrl == undefined || this.options.webServiceProdUrl == null ? this.props.webServiceProdUrl : this.options.webServiceProdUrl;
	}
	writing() {
		this._buildTemplateData();
		this._createStandardPluginFromTemplate();
	}

	install() {
		if(globalfs.existsSync(path.join(this._pluginSourceLocation(), 'Controllers', this.templatedata.Name + 'Controller.cs'))) {
			globalfs.unlinkSync(path.join(this._pluginSourceLocation(), 'Controllers', this.templatedata.Name + 'Controller.cs'));
		}
		
		if(globalfs.existsSync(path.join(this._pluginSourceLocation(), 'Views', this.templatedata.Name))) {
			this._removeDirectory(path.join(this._pluginSourceLocation(), 'Views', this.templatedata.Name));
		}
		if(globalfs.existsSync(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels', this.templatedata.Name + 'ViewModel.cs'))){
			globalfs.unlinkSync(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels', this.templatedata.Name + 'ViewModel.cs'));
		}
		if(globalfs.existsSync(path.join(this._pluginSourceLocation(), 'Scripts', 'IBI.' + this.templatedata.Name + '.js'))){
			globalfs.unlinkSync(path.join(this._pluginSourceLocation(), 'Scripts', 'IBI.' + this.templatedata.Name + '.js'));
		}
		
		globalfs.renameSync(path.join(this._pluginSourceLocation(), 'Controllers', 'PluginTemplateController.cs'), path.join(this._pluginSourceLocation(), 'Controllers', this.templatedata.Name + 'Controller.cs'));
		globalfs.renameSync(path.join(this._pluginSourceLocation(), 'Views', 'PluginTemplate'), path.join(this._pluginSourceLocation(), 'Views', this.templatedata.Name));
		globalfs.renameSync(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels', 'PluginTemplateViewModel.cs'), path.join(this._pluginSourceLocation(), 'Models', 'ViewModels', this.templatedata.Name + 'ViewModel.cs'));
		globalfs.renameSync(path.join(this._pluginSourceLocation(), 'Scripts', 'IBI.PluginTemplate.js'), path.join(this._pluginSourceLocation(), 'Scripts', 'IBI.' + this.templatedata.Name + '.js'));
		
		
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Content', 'Images'));
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Enums'));
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Models', 'Entities'));
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Services', 'Interfaces'));
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Views', 'Home'));
		ibigenerator.addNewFolderToTFS(path.join(this.templatedata.SourceLoc, 'Plugins', this.templatedata.Name));
	}
	
	/*
		Removes a directory and the all of the files below
	 */
	_removeDirectory(dir, file){
		var p = file? path.join(dir,file):dir;
		if(globalfs.lstatSync(p).isDirectory()){
			globalfs.readdirSync(p).forEach(this._removeDirectory.bind(null,p))
			globalfs.rmdirSync(p)
		}
		else globalfs.unlinkSync(p)
	}

	_createStandardPluginFromTemplate(){
		this.fs.copyTpl(
		  this.templatePath('IBI.PluginTemplate.Plugin.sln'),
		  path.join(this.templatedata.SourceLoc, 'Plugins', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Plugin.sln'),
		  this.templatedata
		);
		this.fs.copyTpl(
		  this.templatePath('IBI.PluginTemplate.Plugin - Development.sln'),
		  path.join(this.templatedata.SourceLoc, 'Plugins', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Plugin - Development.sln'),
		  this.templatedata
		);
		//create the master solution file if available
		if(this.options.createMasterSolution){		
			this.fs.copyTpl(
			  this.templatePath('IBI.PluginTemplate.Plugin - Master.sln'),
			  path.join(this.templatedata.SourceLoc, 'Plugins', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Plugin - Master.sln'),
			  this.templatedata
			);
		}
		this._runTemplateOnFolder('App_Start', this._pluginSourceLocation());
		this._runTemplateOnFolder('ConfigSettings', this._pluginSourceLocation());
		this._runTemplateOnFolder('Content', this._pluginSourceLocation());
		this._runTemplateOnFolder('Controllers', this._pluginSourceLocation());
		this._runTemplateOnFolder('Models', this._pluginSourceLocation());
		this._runTemplateOnFolder('Properties', this._pluginSourceLocation());
		this._runTemplateOnFolder('Scripts', this._pluginSourceLocation());
		this._runTemplateOnFolder('Services', this._pluginSourceLocation());
		this._runTemplateOnFolder('Utils', this._pluginSourceLocation());
		this._runTemplateOnFolder('Views', this._pluginSourceLocation());
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Content', 'Images'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Enums'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'Entities'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Services', 'Interfaces'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Views', 'Home'));
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/*.config'),path.join(this._pluginSourceLocation()),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/PluginTemplate.Plugin.cs'), path.join(this._pluginSourceLocation(), this.templatedata.Name + '.Plugin.cs'), this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/PluginTemplate.Installer.cs'), path.join(this._pluginSourceLocation(), this.templatedata.Name + '.Installer.cs'), this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/PluginTemplateLogger.cs'), path.join(this._pluginSourceLocation(), this.templatedata.Name + 'Logger.cs'), this.templatedata);		
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/Global.asax.cs'), path.join(this._pluginSourceLocation(), 'Global.asax.cs'), this.templatedata);
		this.fs.copy(this.templatePath('IBI.PluginTemplate.Plugin/favicon.ico'),path.join(this._pluginSourceLocation(), 'favicon.ico'));
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/IBI.PluginTemplate.Plugin.csproj'),path.join(this._pluginSourceLocation(), 'IBI.' + this.templatedata.Name + '.Plugin.csproj'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.PluginTemplate.Plugin/Global.asax'),path.join(this._pluginSourceLocation(), 'Global.asax'),this.templatedata);
	}
};

