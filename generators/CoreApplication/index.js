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
		  this.templatePath('IBI.CoreTemplate.Application/' + folderName + '/**/*'),
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
		//ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Models', 'Entities', 'Base'));
		//ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels'));
		//ibigenerator.addfolder(path.join(this._pluginSourceLocation(), 'Services', 'Interfaces'));
		//ibigenerator.addNewFolderToTFS(path.join(this.templatedata.SourceLoc, 'Applications', this.templatedata.Name));
	}

	_createStandardPluginFromTemplate(){
		this.fs.copyTpl(
		  this.templatePath('IBI.CoreTemplate.Application.sln'),
		  path.join(this.templatedata.SourceLoc, 'Applications', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Application.sln'),
		  this.templatedata
		);
		this._runTemplateOnFolder('.vscode', this._pluginSourceLocation());
		this._runTemplateOnFolder('Controllers', this._pluginSourceLocation());
		this._runTemplateOnFolder('Models', this._pluginSourceLocation());
		this._runTemplateOnFolder('Properties', this._pluginSourceLocation());
		this._runTemplateOnFolder('Services', this._pluginSourceLocation());
		this._runTemplateOnFolder('Utils', this._pluginSourceLocation());
		this._runTemplateOnFolder('Views', this._pluginSourceLocation());
		this.fs.copy(this.templatePath('IBI.CoreTemplate.Application/wwwroot/**/*'),path.join(this._pluginSourceLocation(), 'wwwroot'));
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/wwwroot/js/IBI.js'),path.join(this._pluginSourceLocation(), 'wwwroot', 'js', 'IBI.js'),this.templatedata);
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'Entities'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'Entities', 'Base'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'Enums'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Models', 'ViewModels'));
		this._createDirectory(path.join(this._pluginSourceLocation(), 'Services', 'Interfaces'));
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/*.config'),path.join(this._pluginSourceLocation()),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/*.json'),path.join(this._pluginSourceLocation()),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/Program.cs'),path.join(this._pluginSourceLocation(), 'Program.cs'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/Startup.cs'),path.join(this._pluginSourceLocation(), 'Startup.cs'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/CoreTemplateTemplate.cs'), path.join(this._pluginSourceLocation(), this.templatedata.Name + 'Template.cs'), this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Application/IBI.CoreTemplate.Application.csproj'),path.join(this._pluginSourceLocation(), 'IBI.' + this.templatedata.Name + '.Application.csproj'),this.templatedata);	
	}
};

