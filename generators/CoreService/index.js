'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
var globalfs = require('fs');
var mkdirp = require('mkdirp');
const moment = require('moment');
const ibigenerator = require('../../ibigenerator');
module.exports = class extends Generator {
	constructor(args, opts) {
		super(args, opts);
		this.option('serviceName', 			{ description: "The Name of the service", 			type: String, alias: "sn" });
		this.option('sourceLocation', 		{ description: "The location of the source code", 	type: String, alias: "sl" });
		this.option('databaseName', 		{ description: "The name of the database", 			type: String, alias: "dn" });
		this.option('databaseServer', 		{ description: "The name of the database service", 	type: String, alias: "ds", default: "jaxdwdv01" });
		this.option('databaseUser', 		{ description: "The user name of the database", 	type: String, alias: "du", default: "Dwsvc" });
		this.option('databasePassword', 	{ description: "The password of the database", 		type: String, alias: "dp", default: "Pass@word1" });
		this.option('databaseProdServer', 	{ description: "The database production server", 	type: String, alias: "dps", default: "SQL02P" });
		this.option('databaseProdUser', 	{ description: "The database production user name", type: String, alias: "dpu", default: "Dwsvc" });
		this.option('databaseProdPassword', { description: "The database production password", 	type: String, alias: "dpp", default: "Pass@word1" });
		
		//reset the files and folders from previous run
		ibigenerator.resetFilesAndFolders();
	}
	
	_serviceSourceLocation(){
		return path.join(this.templatedata.SourceLoc, 'Services', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Service');
	}
	_createDirectory(directory){
		if(!globalfs.existsSync(directory)){ mkdirp.sync(directory); }
	}
	_buildTemplateData() {
		this.templatedata = {};
		this.templatedata.Version = ibigenerator.currentVersion();
		this.templatedata.TodaysDate = moment().format("YYYY-MM-DD, hh:mm A");
		this.templatedata.Name = this.options.serviceName;
		this.templatedata.SourceLoc = this.options.sourceLocation;
		this.templatedata.DatabaseName = this.options.databaseName;
		this.templatedata.DatabaseServer = this.options.databaseServer;
		this.templatedata.DatabaseUser = this.options.databaseUser;
		this.templatedata.DatabasePassword = this.options.databasePassword;
		this.templatedata.DatabaseProdServer = this.options.databaseProdServer;
		this.templatedata.DatabaseProdUser = this.options.databaseProdUser;
		this.templatedata.DatabaseProdPassword = this.options.databaseProdPassword;
	}
	_runTemplateOnFolder(folderName, sourceLocation){
		this.fs.copyTpl(
		  this.templatePath('IBI.CoreTemplate.Service/' + folderName + '/**/*'),
		  path.join(sourceLocation, folderName),
		  this.templatedata
		);
	}
	writing() {
		this._buildTemplateData();
		this._createStandardServiceFromTemplate();
	}
	_createStandardServiceFromTemplate(){
		this.fs.copyTpl(
		  this.templatePath('IBI.CoreTemplate.Service.sln'),
		  path.join(this.templatedata.SourceLoc, 'Services', this.templatedata.Name, 'Trunk', 'IBI.' + this.templatedata.Name + '.Service.sln'),
		  this.templatedata
		);
		this._runTemplateOnFolder('Controllers', this._serviceSourceLocation());
		this._runTemplateOnFolder('Core', this._serviceSourceLocation());
		this._runTemplateOnFolder('Entities', this._serviceSourceLocation());
		this._createDirectory(path.join(this._serviceSourceLocation(), 'Entities', 'Enum'));
		this._runTemplateOnFolder('Properties', this._serviceSourceLocation());
		this._runTemplateOnFolder('Repositories', this._serviceSourceLocation());
		this._runTemplateOnFolder('Services', this._serviceSourceLocation());
		this._createDirectory(path.join(this._serviceSourceLocation(), 'wwwroot'));
		this._createDirectory(path.join(this._serviceSourceLocation(), 'Models'));
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Service/IBI.CoreTemplate.Service.csproj'),path.join(this._serviceSourceLocation(), 'IBI.' + this.templatedata.Name + '.Service.csproj'),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Service/*.cs'),path.join(this._serviceSourceLocation()),this.templatedata);
		this.fs.copyTpl(this.templatePath('IBI.CoreTemplate.Service/*.json'),path.join(this._serviceSourceLocation()),this.templatedata);
	}
	install() {
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Entities', 'Base'));
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Entities', 'Enum'));
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Repositories', 'Interfaces', 'Base'));
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Repositories', 'Base'));
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Services', 'Interfaces', 'Base'));
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Services', 'Base'));
		//ibigenerator.addfolder(path.join(this._serviceSourceLocation(), 'Models'));
		//ibigenerator.addNewFolderToTFS(path.join(this.templatedata.SourceLoc, 'Services', this.templatedata.Name));
	}
};
