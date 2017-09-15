'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
const globalfs = require('fs');
const ibigenerator = require('../../ibigenerator');
var newFiles = [];
module.exports = class extends Generator {
  constructor(args, opts) {  
    super(args, opts);
	this.option('projectname', { description: "The name of the project (plugin or service) *required", type: String, alias: "pn" });
	this.option('location', { description: "The folder location of the root application framework code *required", type: String, alias: "sl" });
	this.option('entityinfo', { description: "A JSON string of some of the entity information", type: String, alias: "ms" });
	ibigenerator.resetFilesAndFolders();
  }
  
  _buildTemplateData(entityInfo) {
	this.templatedata = {};
	this.templatedata.entityinfo = entityInfo;
	this.templatedata.projectname = this.options.projectname;
	this.templatedata.columns =  entityInfo.Columns;
  }
    
  _writeFile(templatePath, filePath, overwrite){
	ibigenerator.checkoutoradd(filePath);
	if(!globalfs.exists(filePath) || overwrite){
		this.fs.copyTpl(templatePath, filePath, this.templatedata);
	}
	if(!globalfs.existsSync(filePath)){
		newFiles.push(filePath);
	}
  }
  
  
  writing() {
	var entityInfo = JSON.parse(this.options.entityinfo);
	for(var i = 0; i < entityInfo.length; i++){
		this._buildTemplateData(entityInfo[i]);
		
		this._writeFile(path.join(this.templatePath(),"Entities", "Base", "Entity.cs"),
						path.join(this.options.location, "Entities", "Base", this.templatedata.entityinfo.PropertyName + '.cs'),
						true);		
		
		this._writeFile(path.join(this.templatePath(),"Entities", "Entity.cs"),
						path.join(this.options.location, "Entities", this.templatedata.entityinfo.PropertyName + '.cs'),
						false);	
		
		this._writeFile(path.join(this.templatePath(),"Services", "RestClient", "RestClient.cs"),
						 path.join(this.options.location, "Services", "RestClient", this.templatedata.entityinfo.PropertyName + 'RestClient.cs'),
						 false);	
		
		this._writeFile(path.join(this.templatePath(),"Services","Interfaces", "ServiceInterface.cs"),
						path.join(this.options.location, "Services", "Interfaces", "I" + this.templatedata.entityinfo.PropertyName + 'Service.cs'),
						false);		
						
		this._writeFile(path.join(this.templatePath(),"Services", "Service.cs"),
						path.join(this.options.location, "Services", this.templatedata.entityinfo.PropertyName + 'Service.cs'),
						false);		
		ibigenerator.writeScaffoldingToProj(this.options.location, newFiles);
	}
  }

  install() {
	ibigenerator.doTfsOperations();
  }
};
