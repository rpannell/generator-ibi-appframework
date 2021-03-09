'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
const globalfs = require('fs');
const moment = require('moment');
const ibigenerator = require('../../ibigenerator');
var newFiles = [];
module.exports = class extends Generator {
  constructor(args, opts) {
    super(args, opts);
    this.option('projectname', {
      description: "The name of the project (plugin or service) *required",
      type: String,
      alias: "pn"
    });
    this.option('location', {
      description: "The folder location of the root application framework code *required",
      type: String,
      alias: "sl"
    });
    this.option('isplugin', {
      description: "True if this is for a plugin false if this is for an application",
      type: Boolean,
      alias: "is"
    });
    this.option('entityinfo', {
      description: "A JSON string of some of the entity information",
      type: String,
      alias: "ms"
    });
    ibigenerator.resetFilesAndFolders();
    newFiles = [];
  }

  _buildTemplateData(entityInfo) {
    var entityTemplateData = {};
    entityTemplateData.entityinfo = entityInfo;
    entityTemplateData.projectname = this.options.projectname;
    entityTemplateData.columns = entityInfo.Columns;
    entityTemplateData.isPlugin = this.options.isplugin;
		entityTemplateData.TodaysDate = moment().format("YYYY-MM-DD, hh:mm A");
    entityTemplateData.Version = ibigenerator.currentVersion();
    return entityTemplateData;
  }

  _writeFile(templatePath, filePath, templateData, overwrite) {
    var that = this;
    ibigenerator.checkoutoradd(filePath).then(function(result){
      ibigenerator.log("Checked out or add" + filePath);
      if (!globalfs.existsSync(filePath)) {
        newFiles.push(filePath);
      }
      if (!globalfs.existsSync(filePath) || overwrite) {
        that.fs.copyTpl(templatePath, filePath, templateData);
      }
    });
  }

  _writeCoreApplicationFiles(entityInfo){
    var coreTemplatePath = path.join(this.templatePath(), "CoreApplication");
    for (var i = 0; i < entityInfo.length; i++) {
      var entityTemplateData = this._buildTemplateData(entityInfo[i]);

      this._writeFile(path.join(coreTemplatePath, "Entities", "Base", "Entity.cs"),
          path.join(this.options.location, "Models", "Entities", "Base", entityTemplateData.entityinfo.PropertyName + '.cs'),
          entityTemplateData,
          true);

      this._writeFile(path.join(coreTemplatePath, "Entities", "Entity.cs"),
          path.join(this.options.location, "Models", "Entities", entityTemplateData.entityinfo.PropertyName + '.cs'),
          entityTemplateData,
          false);  
          
      this._writeFile(path.join(coreTemplatePath, "Services", "Interfaces", "ServiceInterface.cs"),
          path.join(this.options.location, "Services", "Interfaces", "I" + entityTemplateData.entityinfo.PropertyName + 'Service.cs'),
          entityTemplateData,
          false);

       this._writeFile(path.join(coreTemplatePath, "Services", "Service.cs"),
          path.join(this.options.location, "Services", entityTemplateData.entityinfo.PropertyName + 'Service.cs'),
          entityTemplateData,
          false);          
    }
  }

  writing() {
    newFiles = []; //clear everything out
    var entityInfo = JSON.parse(this.options.entityinfo);
    var projectGenieVersion = ibigenerator.getGenieVersionFromProj(this.options.location);
    var applicationVersion = ibigenerator.getApplicationVersionFromProj(this.options.location);

    if(applicationVersion != null && applicationVersion != ""){
      //.NET CORE VErsion
      this._writeCoreApplicationFiles(entityInfo);
    } else {
      /*
       * ASP.NET Version
       */
      for (var i = 0; i < entityInfo.length; i++) {
        var entityTemplateData = this._buildTemplateData(entityInfo[i]);
        
        this._writeFile(path.join(this.templatePath(), "Entities", "Base", "Entity.cs"),
          path.join(this.options.location, "Models", "Entities", "Base", entityTemplateData.entityinfo.PropertyName + '.cs'),
          entityTemplateData,
          true);

        this._writeFile(path.join(this.templatePath(), "Entities", "Entity.cs"),
          path.join(this.options.location, "Models", "Entities", entityTemplateData.entityinfo.PropertyName + '.cs'),
          entityTemplateData,
          false);

        this._writeFile(path.join(this.templatePath(), "Services", "RestClient", "RestClient.cs"),
          path.join(this.options.location, "Services", "RestClient", entityTemplateData.entityinfo.PropertyName + 'RestClient.cs'),
          entityTemplateData,
          false);

        this._writeFile(path.join(this.templatePath(), "Services", "Interfaces", "ServiceInterface.cs"),
          path.join(this.options.location, "Services", "Interfaces", "I" + entityTemplateData.entityinfo.PropertyName + 'Service.cs'),
          entityTemplateData,
          false);

        this._writeFile(path.join(this.templatePath(), "Services", "Service.cs"),
          path.join(this.options.location, "Services", entityTemplateData.entityinfo.PropertyName + 'Service.cs'),
          entityTemplateData,
          false);
        
      }
    }

    ibigenerator.writeScaffoldingToProj(this.options.location, newFiles);
  }

  install() {
    ibigenerator.doTfsOperations().then(function(resolve) { ibigenerator.log("Finished writing files")});
  }
};
