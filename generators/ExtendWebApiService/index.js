'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const path = require('path');
const globalfs = require('fs');
const moment = require('moment');
const ibigenerator = require('../../ibigenerator');
const ejs = require('ejs');
const HOOKSTRING = "/* GENIE HOOK */"
var newFiles = [];
module.exports = class extends Generator {
  constructor(args, opts) {
    super(args, opts);
    this.option('entityname', {
      description: "The name of the entity to extend",
      type: String,
      alias: "en"
    });
    this.option('functionname', {
      description: "The name of the function to add",
      type: String,
      alias: "fn"
    });
    this.option('serviceLocation', {
      description: "The folder location of the root application framework code *required",
      type: String,
      alias: "sl"
    });
    this.option('pluginLocation', {
      description: "The folder location of the root application framework code *required",
      type: String,
      alias: "pl"
    });
    this.option('functionInfo', {
      description: "A JSON string of some of the custom function information to add",
      type: String,
      alias: "ms"
    });
    this.option('isList', {
      description: "A JSON string of some of the custom function information to add",
      type: Boolean,
      alias: "il"
    });
    this.option('isPost', {
      description: "A JSON string of some of the custom function information to add",
      type: Boolean,
      alias: "ip"
    });
  }

  _buildTemplateData() {
    this.templatedata = {};
    this.templatedata.entityname = this.options.entityname;
    this.templatedata.functionname = this.options.functionname;
    this.templatedata.TodaysDate = moment().format("YYYY-MM-DD, hh:mm A");
    this.templatedata.Version = ibigenerator.currentVersion();
    this.templatedata.ControllerPath = "";
    this.templatedata.FunctionCall = "";
    this.templatedata.FunctionDefinition = "";
    this.templatedata.IsPost = this.options.isPost;
    this.templatedata.IsList = this.options.isList;
    this.templatedata.Return = this.options.isList ?
      "System.Collections.Generic.List<" + this.templatedata.entityname + ">" :
      this.templatedata.entityname;

    if (this.options.functionInfo != undefined && this.options.functionInfo != null && this.options.functionInfo != "") {
      var functionParameters = JSON.parse(this.options.functionInfo);
      for (var i = 0; i < functionParameters.length; i++) {
        var parameter = functionParameters[i];

        this.templatedata.FunctionCall += ((i > 0 ? ", " : "") + parameter.Name);
        this.templatedata.FunctionDefinition += ((i > 0 ? ", " : "") + parameter.Type + " " + parameter.Name);
        this.templatedata.ControllerPath += ("/{" + parameter.Name + "}");
      }
    }
    ibigenerator.log('Entity Scaffolding Data', this.templatedata);
    ibigenerator.log('Options', this.options);
  }

  _writeFile(filePath, contents) {
    try {
      var that = this;
      ibigenerator.log('Writing', filePath);
      ibigenerator.checkoutoradd(filePath).then(function(result){
        if (globalfs.existsSync(filePath)) {
          that.fs.write(filePath, contents);
        }
      });
    } catch (err) {
      ibigenerator.log(err);
    }
  }

  _templateFile(templatePath, fileName) {
    var that = this;
    var currentFile = "";
    ejs.renderFile(templatePath, this.templatedata, {}, function (err, str) {
      try {
        currentFile = that.fs.read(fileName, { defaults: "" });
        if (currentFile != "") {
          currentFile = currentFile.replace(HOOKSTRING, str + "\n\t\t" + HOOKSTRING);
          that._writeFile(fileName, currentFile);
        }
      } catch (err) { ibigenerator.log("Error Reading Existing File", err); }
    });
  }
  writing() {
    this._buildTemplateData();

    if (this.options.serviceLocation != undefined && this.options.serviceLocation != null && this.options.serviceLocation != "") {
      var serviceGenerateVersion = ibigenerator.getGenieVersionFromProj(this.options.serviceLocation);
      ibigenerator.log("", "Has Service Location");
      this._templateFile(path.join(this.templatePath(), "Repositories", "Interfaces", "CustomFunction.ejs"),
        path.join(this.options.serviceLocation, "Repositories", "Interfaces", "I" + this.templatedata.entityname + "Repository.cs"));

      this._templateFile(path.join(this.templatePath(), "Repositories", "CustomFunction.ejs"),
        path.join(this.options.serviceLocation, "Repositories", this.templatedata.entityname + "Repository.cs"));

      this._templateFile(path.join(this.templatePath(), "Services", "Interfaces", "CustomFunction.ejs"),
        path.join(this.options.serviceLocation, "Services", "Interfaces", "I" + this.templatedata.entityname + "Service.cs"));

      this._templateFile(path.join(this.templatePath(), "Services", "CustomFunction.ejs"),
        path.join(this.options.serviceLocation, "Services", this.templatedata.entityname + "Service.cs"));
      
      //create the controller based on the version of the scaffolding that created the project
      this._templateFile(path.join(this.templatePath(), "Controllers", serviceGenerateVersion != null && serviceGenerateVersion >= "1.1.24" 
                                                                          ? "CustomFunction.ejs" 
                                                                          : "OlderCustomFunction.ejs"),
          path.join(this.options.serviceLocation, "Controllers", this.templatedata.entityname + "Controller.cs"));
      
    }

    if (this.options.pluginLocation != undefined && this.options.pluginLocation != null && this.options.pluginLocation != "") {
      ibigenerator.log("", "Has Plugin Location");
      this._templateFile(path.join(this.templatePath(), "Plugin", "Services", "Interfaces", "CustomFunction.ejs"),
        path.join(this.options.pluginLocation, "Services", "Interfaces", "I" + this.templatedata.entityname + "Service.cs"));

      this._templateFile(path.join(this.templatePath(), "Plugin", "Services", "CustomFunction.ejs"),
        path.join(this.options.pluginLocation, "Services", this.templatedata.entityname + "Service.cs"));
    }
  }

  install() {}
};
