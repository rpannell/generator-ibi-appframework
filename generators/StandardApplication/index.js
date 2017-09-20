'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
var globalfs = require('fs');
var mkdirp = require('mkdirp');
const ibigenerator = require('../../ibigenerator');
module.exports = class extends Generator {
	constructor(args, opts) {
		super(args, opts);
		
		this.option('pluginName', { description: "The Name of the Application", type: String, alias: "pn" });
		this.option('sourceLocation', { description: "The location of the source code", type: String, alias: "sl" });
		this.option('createMasterSolution', { description: "Tells the solution to create the master solution with a service with the same name as the plugin", type: Boolean, default: false, alias: "ms" });
		
		this.option('webServiceUrl', { description: "The URL of the development web service", type: String, default: false, alias: "wsurl" });
		this.option('webServiceTestUrl', { description: "The URL of the development web service", type: String, default: false, alias: "wsurl" });
		this.option('webServiceProdUrl', { description: "The URL of the development web service", type: String, default: false, alias: "wsurl" });
		ibigenerator.resetFilesAndFolders();
	}

	_pluginSourceLocation(){
		return path.join(this.templatedata.SourceLoc, 'Applications', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Application');
	}
	_runTemplateOnFolder(folderName, sourceLocation){
		console.log("Folder: " + folderName);
		this.fs.copyTpl(
		  this.templatePath('IBI.ExternalPluginTemplate.Application/' + folderName + '/**/*'),
		  path.join(sourceLocation, folderName),
		  this.templatedata
		);
	}
	_createDirectory(directory){
		if(!globalfs.existsSync(directory)){ mkdirp.sync(directory); }
	}
	_buildTemplateData() {
		this.templatedata = {};
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

		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Models', 'Entities', 'Base'));
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels'));
		ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Services', 'Interfaces'));
		ibigenerator.addNewFolderToTFS(path.join(this.templatedata.SourceLoc, 'Applications', this.templatedata.Name));
	}

	_createStandardPluginFromTemplate(){
		this.fs.copyTpl(
		  this.templatePath('IBI.ExternalPluginTemplate.Application.sln'),
		  path.join(this.templatedata.SourceLoc, 'Applications', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Application.sln'),
		  this.templatedata
		);
		this._runTemplateOnFolder('App_Start', this._pluginSourceLocation());
		this._runTemplateOnFolder('Content', this._pluginSourceLocation());
		this._runTemplateOnFolder('Controllers', this._pluginSourceLocation());
		this.fs.copy(this.templatePath('IBI.ExternalPluginTemplate.Application/fonts/*.*'),path.join(this._pluginSourceLocation(), "fonts"));
		this._runTemplateOnFolder('Models', this._pluginSourceLocation());
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'Entities', 'Base'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels'));
		this._runTemplateOnFolder('Properties', this._pluginSourceLocation());
		this._runTemplateOnFolder('Scripts', this._pluginSourceLocation());
		this._runTemplateOnFolder('Services', this._pluginSourceLocation());
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Services', 'Interfaces'));
		this._runTemplateOnFolder('Utils', this._pluginSourceLocation());
		this._runTemplateOnFolder('Views', this._pluginSourceLocation());
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/*.config'),path.join(this._pluginSourceLocation()),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/ExternalPluginTemplate.cs'), path.join(this._pluginSourceLocation(), this.templatedata.Name + '.cs'), this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/ExternalPluginTemplateLogger.cs'), path.join(this._pluginSourceLocation(), this.templatedata.Name + 'Logger.cs'), this.templatedata);		
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/Global.asax.cs'), path.join(this._pluginSourceLocation(), 'Global.asax.cs'), this.templatedata);
		this.fs.copy(this.templatePath('IBI.ExternalPluginTemplate.Application/favicon.ico'),path.join(this._pluginSourceLocation(), 'favicon.ico'));
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/IBI.ExternalPluginTemplate.Application.csproj'),path.join(this._pluginSourceLocation(), 'IBI.' + this.templatedata.Name + '.Application.csproj'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/IBI.ExternalPluginTemplate.Application.csproj.user'),path.join(this._pluginSourceLocation(), 'IBI.' + this.templatedata.Name + '.Application.csproj.user'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/Global.asax'),path.join(this._pluginSourceLocation(), 'Global.asax'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.ExternalPluginTemplate.Application/Startup.cs'),path.join(this._pluginSourceLocation(), 'Startup.cs'),this.templatedata);
	}
};

