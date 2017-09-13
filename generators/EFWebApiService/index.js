'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
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
  
  _buildTemplateData() {
	var entityInfo = JSON.parse(this.options.entityinfo);
	this.templatedata = {};
	this.templatedata.entityinfo = entityInfo;
	this.templatedata.projectname = this.options.projectname;
	this.templatedata.primarykey = entityInfo.PrimaryKey;
	this.templatedata.columns =  entityInfo.Columns;
  }
  
  _writeFile(templatePath, filePath, overwrite){
	ibigenerator.checkoutoradd(filePath);
	if(!this.fs.exists(filePath) || overwrite){
		this.fs.copyTpl(templatePath, filePath, this.templatedata);
	}
  }
  
  
  writing() {
	this._buildTemplateData();
	
	this._writeFile(path.join(this.templatePath(),"Entity", "Base", "EntityName.cs"),
					path.join(this.options.location, "Entities", "Base", this.templatedata.entityinfo.PropertyName + '.cs'),
					true);		
	
	this._writeFile(path.join(this.templatePath(),"Entity", "EntityName.cs"),
					path.join(this.options.location, "Entities", this.templatedata.entityinfo.PropertyName + '.cs'),
					false);	
	
	//create base repository
	this._writeFile(path.join(this.templatePath(),"Repositories", "Base", "BaseRepository.cs"),
					path.join(this.options.location, "Repositories", "Base", this.templatedata.entityinfo.PropertyName + 'Repository.cs'),
					true);	
	
	//create repository
	this._writeFile(path.join(this.templatePath(),"Repositories", "EntityRepository.cs"),
					path.join(this.options.location, "Repositories", this.templatedata.entityinfo.PropertyName + 'Repository.cs'),
					false);		
	  
	this._writeFile(path.join(this.templatePath(),"Repositories", "Interfaces",  "Base", "BaseRepositoryInterface.cs"),
					path.join(this.options.location, "Repositories", "Interfaces", "Base", "I" + this.templatedata.entityinfo.PropertyName + 'Repository.cs'),
					true);		  

	this._writeFile(path.join(this.templatePath(),"Repositories", "Interfaces", "RepositoryInterface.cs"),
					path.join(this.options.location, "Repositories", "Interfaces" , "I" + this.templatedata.entityinfo.PropertyName + 'Repository.cs'),
					false);		  

	this._writeFile(path.join(this.templatePath(),"Services", "Base", "BaseService.cs"),
					path.join(this.options.location, "Services", "Base", this.templatedata.entityinfo.PropertyName + 'Service.cs'),
					true);	
	    
	this._writeFile(path.join(this.templatePath(),"Services", "Service.cs"),
					path.join(this.options.location, "Services", this.templatedata.entityinfo.PropertyName + 'Service.cs'),
					false);	
					
	this._writeFile(path.join(this.templatePath(),"Services", "Interfaces",  "Base", "BaseServiceInterface.cs"),
					path.join(this.options.location, "Services", "Interfaces", "Base", "I" + this.templatedata.entityinfo.PropertyName + 'Service.cs'),
					true);	

	this._writeFile(path.join(this.templatePath(),"Services", "Interfaces", "ServiceInterface.cs"),
					path.join(this.options.location, "Services", "Interfaces" , "I" + this.templatedata.entityinfo.PropertyName + 'Service.cs'),
					false);	  
	  
	this._writeFile(path.join(this.templatePath(),"Controllers", "Base", "BaseController.cs"),
					path.join(this.options.location, "Controllers", "Base", this.templatedata.entityinfo.PropertyName + 'Controller.cs'),
					true);
	
	this._writeFile(path.join(this.templatePath(),"Controllers", "Controller.cs"),
					path.join(this.options.location, "Controllers", this.templatedata.entityinfo.PropertyName + 'Controller.cs'),
					false);
  }

  install() {
	ibigenerator.doTfsOperations();
	ibigenerator.writeScaffoldingToProj(this.options.location);
  }
};
