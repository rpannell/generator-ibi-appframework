'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
var globalfs = require('fs');
var mkdirp = require('mkdirp');
const moment = require('moment');
const ibigenerator = require('../../ibigenerator');
var newFiles = [];
module.exports = class extends Generator {
	constructor(args, opts) {
		super(args, opts);
		this.option('controllerName', { description: "The Name of the controller", type: String, alias: "cn" });
		this.option('pluginSourceLocation', { description: "The Location of the plugin", type: String, alias: "loc" });
		this.option('pluginName', { description: "The Location of the plugin name", type: String, alias: "pn" });
		//reset the files and folders from previous run
		newFiles = [];
		ibigenerator.resetFilesAndFolders();
	}
	
	_writeFile(templatePath, filePath, templateData, overwrite){
		var that = this;
		ibigenerator.checkoutoradd(filePath).then(function(result){
			ibigenerator.log("Checked out or add" + filePath);
			if(!globalfs.existsSync(filePath) || overwrite){
				that.fs.copyTpl(templatePath, filePath, templateData);
			}
			if(!globalfs.existsSync(filePath)){
				newFiles.push(filePath);
			}
		});
  	}

	_buildTemplateData() {
		this.templatedata = {};
		this.templatedata.Version = ibigenerator.currentVersion();
		this.templatedata.TodaysDate = moment().format("YYYY-MM-DD, hh:mm A");
		this.templatedata.ControllerName = this.options.controllerName;
		this.templatedata.PluginName = this.options.pluginName;
	}

	writing() {
		newFiles = [];
		ibigenerator.log("Create New Controller", this.options.controllerName);
		this._buildTemplateData();

		this._writeFile(path.join(this.templatePath(), "Controller.cs"),
						path.join(this.options.pluginSourceLocation, "Controllers", this.options.controllerName + 'Controller.cs'),
						this.templatedata,
						false);	
		this._writeFile(path.join(this.templatePath(), "InitialViewModel.cs"),
						path.join(this.options.pluginSourceLocation, "Models", "ViewModels", this.options.controllerName, this.options.controllerName + "ViewModel.cs"),
						this.templatedata,
						false);	

		this._writeFile(path.join(this.templatePath(), "View.cshtml"),
						path.join(this.options.pluginSourceLocation, "Views", this.options.controllerName, 'Index.cshtml'),
						this.templatedata,
						false);	

		
		var genieVersion = ibigenerator.getGenieVersionFromProj(this.options.pluginSourceLocation);
		var projVersion = ibigenerator.getProjectVersionFromProj(this.options.pluginSourceLocation);
		ibigenerator.log("Genie Version in generator: ", genieVersion);
		ibigenerator.log("Proj Version in generator: ", projVersion);
		ibigenerator.writeScaffoldingToProj(this.options.pluginSourceLocation, newFiles);
	}
				  
	install() {
		ibigenerator.doTfsOperations().then(function(result){ ibigenerator.log("Done Adding TFS files");});
	}
};
