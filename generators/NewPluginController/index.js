'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
var globalfs = require('fs');
var mkdirp = require('mkdirp');
const ibigenerator = require('../../ibigenerator');
var compileFiles = [];
var contentFiles = [];
module.exports = class extends Generator {
	constructor(args, opts) {
		super(args, opts);
		this.option('controllerName', { description: "The Name of the controller", type: String, alias: "cn" });
		this.option('pluginSourceLocation', { description: "The Location of the plugin", type: String, alias: "loc" });
		this.option('pluginName', { description: "The Location of the plugin name", type: String, alias: "pn" });
		//reset the files and folders from previous run
		compileFiles = [];
		contentFiles = [];
		ibigenerator.resetFilesAndFolders();
	}
	
	/**
	 * Write the file using the template data to replace some tags
	 * @param  {string} templatePath - the path to the template file to write
	 * @param  {string} filePath - the output file path
	 * @param  {object} templateData - the object used as replacement in the template 
	 * @param  {boolean} overwrite - true to overwrite the file if it already exists
	 * @param  {boolean} isContentFile - true if this is a content file on the proj file
	 */
	writeFile(templatePath, filePath, templateData, overwrite, isContentFile){
		var that = this;
		ibigenerator.checkoutoradd(filePath).then(function(result){
			ibigenerator.log("Checked out or add" + filePath);
			if(!globalfs.existsSync(filePath) || overwrite){
				that.fs.copyTpl(templatePath, filePath, templateData);
			}
			//if the file doesn't exists at this moment then we 
			//need to add to the project file
			if(!globalfs.existsSync(filePath)){
				if(isContentFile) contentFiles.push(filePath);
				else compileFiles.push(filePath);
			}
		});
  	}

	/**
	 * Builds the template data object
	 * this builds that object that is used to 
	 * write the file using ejs
	 */
	buildTemplateData() {
		this.templatedata = {};
		this.templatedata.Version = ibigenerator.currentVersion();
		this.templatedata.TodaysDate = ibigenerator.getCurrentTime();
		this.templatedata.ControllerName = this.options.controllerName;
		this.templatedata.PluginName = this.options.pluginName;
	}

	writing() {
		compileFiles = [];
		ibigenerator.log("Create New Controller", this.options.controllerName);
		this.buildTemplateData();

		this.writeFile(path.join(this.templatePath(), "Controller.cs"),
						path.join(this.options.pluginSourceLocation, "Controllers", this.options.controllerName + 'Controller.cs'),
						this.templatedata,
						false,
						false);	

		this.writeFile(path.join(this.templatePath(), "InitialViewModel.cs"),
						path.join(this.options.pluginSourceLocation, "Models", "ViewModels", this.options.controllerName, this.options.controllerName + "ViewModel.cs"),
						this.templatedata,
						false,
						false);	

		this.writeFile(path.join(this.templatePath(), "View.cshtml"),
						path.join(this.options.pluginSourceLocation, "Views", this.options.controllerName, 'Index.cshtml'),
						this.templatedata,
						false,
						true);	

		ibigenerator.writeCompileFilesToProj(this.options.pluginSourceLocation, compileFiles);
		ibigenerator.writeContentFilesToProj(this.options.pluginSourceLocation, contentFiles);
	}
				  
	install() {
		ibigenerator.doTfsOperations().then(function(result){ ibigenerator.log("Done Adding TFS files");});
	}
};
