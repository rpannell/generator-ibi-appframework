'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
const globalfs = require('fs');
const moment = require('moment');
const ibigenerator = require('../../ibigenerator');
var newFiles = [];
const winston = require('winston');  
winston.configure({
	transports: [
	  new (winston.transports.File)({ filename: path.join(process.env.APPDATA, "ApplicationFrameworkManagement", "logs", "app.log") })
	]
});
module.exports = class extends Generator {
  constructor(args, opts) {  
    super(args, opts);
	this.option('projectname', { description: "The name of the project (plugin or service) *required", type: String, alias: "pn" });
	this.option('location', { description: "The folder location of the root application framework code *required", type: String, alias: "sl" });
	this.option('entityinfo', { description: "A JSON string of some of the entity information", type: String, alias: "ms" });
	ibigenerator.resetFilesAndFolders();
  }

  _searchAbleType(type){
	var attr = "";
	switch(type){
		case "Contains":
			attr = "SearchAbleType.Contains";
            break;

		case "Starts With":
			attr = "SearchAbleType.StartsWith";
			break;

		case "Equal":
			attr = "SearchAbleType.Equal";
			break;

		case "Not Equal":
			attr = "SearchAbleType.NotEqual";
			break;

		case "Ends With":
			attr = "SearchAbleType.EndsWith";
			break;

		case "Greater Than":
			attr = "SearchAbleType.GreaterThan";
			break;

		case "Greater Than or Equal":
			attr = "SearchAbleType.GreaterThanEqual";
			break;

		case "Less Than":
			attr = "SearchAbleType.LessThan";
			break;

		case "Less Than or Equal":
			attr = "SearchAbleType.LessThanEqual";
			break;

		default:
			attr = "";
			break;
	  }
	  return attr;
  }  
  
  _buildTemplateData(entityInfo) {
	for(var i = 0; i < entityInfo.Columns.length; i++){
		entityInfo.Columns[i].SearchTypeAttribute = this._searchAbleType(entityInfo.Columns[i].Search);
	}
	this.templatedata = {};
	this.templatedata.entityinfo = entityInfo;
	this.templatedata.projectname = this.options.projectname;
	this.templatedata.primarykey = entityInfo.PrimaryKey;
	this.templatedata.columns =  entityInfo.Columns;
	this.templatedata.TodaysDate = moment().format("YYYY-MM-DD, hh:mm A");
	this.templatedata.Version = ibigenerator.currentVersion();
	ibigenerator.log('Entity Scaffolding Data', this.templatedata);
  }
  
  _writeFile(templatePath, filePath, overwrite){
	var that = this;
	ibigenerator.checkoutoradd(filePath).then(function(result){
		ibigenerator.log("Checked out or add" + filePath);
		if(!globalfs.existsSync(filePath) || overwrite){
			that.fs.copyTpl(templatePath, filePath, that.templatedata);
		}
		if(!globalfs.existsSync(filePath)){
			newFiles.push(filePath);
		}
	});
  }
  
  writing() {
	//walk each entity and running scaffolding for each entity
	newFiles = [];
	ibigenerator.log("Entity Data Before parsing the string", this.options.entityinfo);
	var entityInfo = JSON.parse(this.options.entityinfo);
	ibigenerator.log("Entity Data After parsing the string", entityInfo);
	for(var i = 0; i < entityInfo.length; i++){
		this._buildTemplateData(entityInfo[i]);
		this._writeFile(path.join(this.templatePath(),"Entity", "Base", "EntityName.cs"),
				path.join(this.options.location, "Entities", "Base", this.templatedata.entityinfo.PropertyName + '.cs'),
				true);		

		this._writeFile(path.join(this.templatePath(),"Entity", "EntityName.cs"),
						path.join(this.options.location, "Entities", this.templatedata.entityinfo.PropertyName + '.cs'),
						false);	

		//create repository
		this._writeFile(path.join(this.templatePath(),"Repositories", "EntityRepository.cs"),
						path.join(this.options.location, "Repositories", this.templatedata.entityinfo.PropertyName + 'Repository.cs'),
						false);			  

		this._writeFile(path.join(this.templatePath(),"Repositories", "Interfaces", "RepositoryInterface.cs"),
						path.join(this.options.location, "Repositories", "Interfaces" , "I" + this.templatedata.entityinfo.PropertyName + 'Repository.cs'),
						false);		  
			
		this._writeFile(path.join(this.templatePath(),"Services", "Service.cs"),
						path.join(this.options.location, "Services", this.templatedata.entityinfo.PropertyName + 'Service.cs'),
						false);	

		this._writeFile(path.join(this.templatePath(),"Services", "Interfaces", "ServiceInterface.cs"),
						path.join(this.options.location, "Services", "Interfaces" , "I" + this.templatedata.entityinfo.PropertyName + 'Service.cs'),
						false);	  
		
		this._writeFile(path.join(this.templatePath(),"Controllers", "Controller.cs"),
						path.join(this.options.location, "Controllers", this.templatedata.entityinfo.PropertyName + 'Controller.cs'),
						false);
	}

	ibigenerator.writeScaffoldingToProj(this.options.location, newFiles);
  }

  install() {
	ibigenerator.doTfsOperations().then(function(result){ ibigenerator.log("Done Adding TFS files" + filePath);});
  }
};
