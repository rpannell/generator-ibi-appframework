'use strict';
const moment = require('moment');
var newFiles = [],
  newContent = [],
  folders = [],
  fs = require('fs'),
  tfs = require('./tfs-unlock'),
  path = require('path'),
  glob = require('glob'),
  xmldom = require('xmldom').DOMParser,
  xmlserial = require('xmldom').XMLSerializer,
  pjson = require('./package.json'),
  helper;

const winston = require('winston');
  winston.configure({
    transports: [new(winston.transports.File)({
    filename: path.join(process.env.APPDATA, "ApplicationFrameworkManagement", "logs", "app.log")
  })]
});

/** initials the TFS as 2017 */
tfs.init({

});

/**
 * The helper class to manage the module
 */
helper = {
  /**
   * Finds the csproj file in a folder
   * @param  {string} sourceLocation - folder location of the project file
   * @returns the file location of the csproj file
   */
  getProjectFile: function (sourceLocation) {
    var files = glob.sync("*.csproj", {
      cwd: sourceLocation
    });
    return files != null && files.length > 0 ? files[0] : "";
  },
  
  /**
   * Write to the log file
   * 
   * @param  {string} extraData - log file 
   * @param  {} object - object data to write to a file
   */
  log: function (extraData, object) {
    winston.info(extraData, object);
  },
  isFileWritable(filePath) {
    var stats = fs.statSync(filePath);
    return (stats["mode"] & 2);
  },
  /**
   * Check out a file from TFS
   * 
   * @param  {string} filePath - file path to check out
   * @returns Promise
   */
  checkout: function (filePath) {
    helper.log("Checkout: " + filePath);
    if (!helper.isFileWritable(filePath)) {
      helper.log("Check file is not writable");
      var paths = [];
      paths.push(filePath);
      return tfs.checkout(paths);
    }
    helper.log("Check file is writable");
    return new Promise(function (resolve) {
      resolve()
    });
  },
  addContent: function (filePath) {
    return new Promise(function (resolve) {
      if (newContent == null || newContent == undefined) {
        newContent = [];
      }
      newContent.push(filePath);
      var dirName = path.dirname(filePath);
      folders.push(dirName);
    });
  },
  addfile: function (filePath) {
    return new Promise(function (resolve) {
      if (newFiles == null || newFiles == undefined) {
        newFiles = [];
      }
      newFiles.push(filePath);
      var dirName = path.dirname(filePath);
      folders.push(dirName);

      resolve("Worked");
    });
  },
  addfolder: function (folderPath) {
    folders.push(folderPath);
  },
  netWebApi: function (sourceLocation, progFile) {
    fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function (err, data) {
      var doc = new xmldom().parseFromString(data, 'text/xml');
      var items = doc.getElementsByTagName('Project');
      var foundIt = false;
      for (var i in items) {
        var item = items[i];
        if (item.documentElement != undefined) {
          if (item.documentElement.tagName == "Project") {
            for (var j = 0; j < item.documentElement.childNodes.length; j++) {
              var e = item.documentElement.childNodes[j];
              if (e.tagName == "ItemGroup" && e.hasChildNodes()) {
                for (var k = 0; k < e.childNodes.length; k++) {
                  if (e.childNodes[k].tagName == "Compile") {
                    foundIt = true;
                    break;
                  }
                }

                if (foundIt) {
                  for (var f = 0; f < newFiles.length; f++) {
                    var file = newFiles[f];
                    console.log("F: " + f);
                    console.log("File: " + file);
                    console.log("New File: " + newFiles[f]);
                    if (file.includes(sourceLocation)) {
                      var newCompile = doc.createElement("Compile");
                      newCompile.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
                      e.appendChild(newCompile);
                    }
                  }


                  break;
                }
              }
            }
          }
        }
      }
      fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));
    });
  },
  netWebApiFiles: function (sourceLocation, progFile, files) {
    fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function (err, data) {
      var doc = new xmldom().parseFromString(data, 'text/xml');
      var items = doc.getElementsByTagName('Project');
      var foundIt = false;
      for (var i in items) {
        var item = items[i];
        if (item.documentElement != undefined) {
          if (item.documentElement.tagName == "Project") {
            for (var j = 0; j < item.documentElement.childNodes.length; j++) {
              var e = item.documentElement.childNodes[j];
              if (e.tagName == "ItemGroup" && e.hasChildNodes()) {
                for (var k = 0; k < e.childNodes.length; k++) {
                  if (e.childNodes[k].tagName == "Compile") {
                    foundIt = true;
                    break;
                  }
                }

                if (foundIt) {
                  for (var f = 0; f < files.length; f++) {
                    var file = files[f];
                    if (file.includes(sourceLocation)) {
                      var newCompile = doc.createElement("Compile");
                      newCompile.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
                      e.appendChild(newCompile);
                    }
                  }


                  break;
                }
              }
            }
          }
        }
      }
      fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));
    });
  },
  standardPluginProj: function (sourceLocation, progFile) {
    fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function (err, data) {
      var doc = new xmldom().parseFromString(data, 'text/xml');
      var items = doc.getElementsByTagName('Project');
      var foundIt = false;
      for (var i in items) {
        var item = items[i];
        if (item.documentElement != undefined) {
          if (item.documentElement.tagName == "Project") {
            for (var j = 0; j < item.documentElement.childNodes.length; j++) {
              var e = item.documentElement.childNodes[j];
              if (e.tagName == "ItemGroup" && e.hasChildNodes()) {
                for (var k = 0; k < e.childNodes.length; k++) {
                  if (e.childNodes[k].tagName == "Compile") {
                    foundIt = true;
                    break;
                  }
                }

                if (foundIt) {
                  for (var f = 0; f < newFiles.length; f++) {
                    var file = newFiles[f];
                    if (file.includes(sourceLocation)) {
                      var newCompile = doc.createElement("Compile");
                      newCompile.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
                      e.appendChild(newCompile);
                    }
                  }


                  break;
                }
              }
            }
          }
        }
      }
      fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));
    });
  },
  /**
   * Adds a list of content files to a project file
   * @param  {string} sourceLocation - the folder location of the project file
   * @param  {string} progFile - the project file
   * @param  {array} contentFiles - the files to add as contet
   */
  addContentToProj: function (sourceLocation, progFile, contentFiles) {
    fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function (err, data) {
      var doc = new xmldom().parseFromString(data, 'text/xml');
      var items = doc.getElementsByTagName('Project');
      var foundIt = false;
      for (var i in items) {
        var item = items[i];
        if (item.documentElement != undefined) {
          if (item.documentElement.tagName == "Project") {
            for (var j = 0; j < item.documentElement.childNodes.length; j++) {
              var e = item.documentElement.childNodes[j];
              if (e.tagName == "ItemGroup" && e.hasChildNodes()) {
                for (var k = 0; k < e.childNodes.length; k++) {
                  if (e.childNodes[k].tagName == "Content") {
                    foundIt = true;
                    break;
                  }
                }

                if (foundIt) {
                  for (var f = 0; f < contentFiles.length; f++) {
                    var file = contentFiles[f];
                    if (file.includes(sourceLocation)) {
                      var newContent = doc.createElement("Content");
                      newContent.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
                      e.appendChild(newContent);
                    }
                  }

                  break;
                }
              }
            }
          }
        }
      }
      fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));
    });
  }
};
/**
 * Log to the standard log file
 * @param  {} extraData
 * @param  {} object
 */
exports.log = function (extraData, object) {
  helper.log(extraData, object);
};

/**
 * Resets the files and contents
 */
exports.resetFilesAndFolders = function () {
  folders = [];
  newFiles = [];
};

/**
 * Adds a file to the list of files to add 
 * to the project file
 * @param  {string} filePath - path of the code file
 */
exports.addfile = function (filePath) {
  helper.addfile(filePath);
};

/**
 * Adds a a content file to the list of file to add
 * to the project file
 * @param  {string} filePath - path of a the content file
 */
exports.addContent = function (filePath) {
  helper.addContent(filePath);
};

/**
 * Adds a folder to the list of folders to check
 * into TFS
 * @param  {string} folderPath - path to a folder
 */
exports.addfolder = function (folderPath) {
  helper.addfolder(folderPath);
};

/**
 * checks out a file from TFS
 * @param  {string} filePath - path to a specific file
 */
exports.checkout = function (filePath) {
  helper.checkout(filePath);
};

/**
 * checks out a file from TFS and or creates the
 * files in TFS
 * @param  {string} filePath - file path to a specific file
 */
exports.checkoutoradd = function (filePath) {
  try {
    helper.log("Looking for: " + filePath);
    if (fs.existsSync(filePath)) {
      helper.log("Checking out: " + filePath);
      return helper.checkout(filePath);
    } else {
      helper.log("Adding: " + filePath);
      return helper.addfile(filePath);
    }
  } catch (error) {
    helper.log("Check out error: " + error);
  }
};

/**
 * checks out to see if the file is already checked out (or writable)
 * before calling TFS
 * @param  {string} filePath - the file path to check
 * @param  {function} callbackFunction - the function to call if the file is writable
 */
exports.canWriteFile = function (filePath, callbackFunction) {
  fs.filePath(file, function (error, stats) {
    if (error) {
      callbackFunctioncb(error, false);
    } else {
      callbackFunction(null, !!(mask & parseInt((stats.mode & parseInt("777", 8)).toString(8)[0])));
    }
  });
}

/**
 * writes the new classes to the project file
 * @param  {string} sourceLocation - the folder location that contains the project
 * @param  {array} serviceFiles - the files to add to the project
 */
exports.writeScaffoldingToProj = function (sourceLocation, serviceFiles) {
  //find the prog file
  glob("*.csproj", {
    cwd: sourceLocation
  }, function (er, files) {
    var progFile = "";
    if (er == null && files != null && files.length > 0) {
      progFile = files[0];
    }

    helper.checkout(path.join(sourceLocation, progFile)).then(function (resolve) {
      helper.log("Done check out of: " + path.join(sourceLocation, progFile) + " update proj file");
      helper.netWebApiFiles(sourceLocation, progFile, serviceFiles);
    });
  });
}
/**
 * write compile time files to the project file
 * @param  {string} sourceLocation - the folder location of the project file
 * @param  {array} serviceFiles - the list of compile time files to add to a project
 */
exports.writeCompileFilesToProj = function (sourceLocation, serviceFiles) {
  var progFile = helper.getProjectFile(sourceLocation);
  helper.checkout(path.join(sourceLocation, progFile)).then(function (resolve) {
    helper.log("Done check out of: " + path.join(sourceLocation, progFile) + " update proj file");
    helper.netWebApiFiles(sourceLocation, progFile, serviceFiles);
  });
}

/**
 * Adds content files to a project file
 * Content files aren't files to be compiled
 * @param  {string} sourceLocation - folder location of the project file
 * @param  {array} contentFiles - the files to add
 */
exports.writeContentFilesToProj = function (sourceLocation, contentFiles) {
  var progFile = helper.getProjectFile(sourceLocation);
  helper.checkout(path.join(sourceLocation, progFile)).then(function (resolve) {
    helper.addContentToProj(sourceLocation, progFile, contentFiles);
  });
}

// exports.writeScaffoldingToPluginProj = function (sourceLocation) {
//   //find the prog file
//   glob("*.csproj", {
//     cwd: sourceLocation
//   }, function (er, files) {
//     var progFile = "";
//     if (er == null && files != null && files.length > 0) {
//       progFile = files[0];
//     }

//     helper.checkout(path.join(sourceLocation, progFile));
//     helper.standardPluginProj(sourceLocation, progFile);
//   });
// }

/**
 * Will get the version of project to
 * so that the developer can template the correct file
 * 
 * @param  {string} sourceLocation - folder that contains the project file
 */
exports.getProjectVersionFromProj = function (sourceLocation) {
  var searchString = "PROJECT VERSION:";
  var projectVersion = "";
  var progFile = helper.getProjectFile(sourceLocation);
  if (progFile == null || progFile == "") return "";
  if (fs.existsSync(path.join(sourceLocation, progFile))) {
    var fileData = fs.readFileSync(path.join(sourceLocation, progFile));
    var regex = new RegExp("^.*" + searchString + ".*$", 'm');
    var matches = regex.exec(fileData);
    projectVersion = matches != null && matches.length > 0 ?
      matches[0].trim().replace(searchString, "").trim() :
      "";
  }

  return projectVersion;
}

/**
 * Will get the version of project to
 * so that the developer can template the correct file
 * 
 * @param  {string} sourceLocation - folder that contains the project file
 */
exports.getServiceVersionFromProj = function (sourceLocation) {
  var searchString = "SERVICE VERSION:";
  var serviceVersion = "";
  var progFile = helper.getProjectFile(sourceLocation);
  if (progFile == null || progFile == "") return "";
  if (fs.existsSync(path.join(sourceLocation, progFile))) {
    var fileData = fs.readFileSync(path.join(sourceLocation, progFile));
    var regex = new RegExp("^.*" + searchString + ".*$", 'm');
    var matches = regex.exec(fileData);
    serviceVersion = matches != null && matches.length > 0 ?
      matches[0].trim().replace(searchString, "").trim() :
      "";
  }

  return serviceVersion;
}


/**
 * Will get the version of project to
 * so that the developer can template the correct file
 * 
 * @param  {string} sourceLocation - folder that contains the project file
 */
exports.getApplicationVersionFromProj = function (sourceLocation) {
  var searchString = "APPLICATION VERSION:";
  var serviceVersion = "";
  var progFile = helper.getProjectFile(sourceLocation);
  if (progFile == null || progFile == "") return "";
  if (fs.existsSync(path.join(sourceLocation, progFile))) {
    var fileData = fs.readFileSync(path.join(sourceLocation, progFile));
    var regex = new RegExp("^.*" + searchString + ".*$", 'm');
    var matches = regex.exec(fileData);
    serviceVersion = matches != null && matches.length > 0 ?
      matches[0].trim().replace(searchString, "").trim() :
      "";
  }

  return serviceVersion;
}


/**
 * Will get the version of the Genie Generator that created the project file
 * so that the developer can template the correct file
 * 
 * @param  {string} sourceLocation - folder that contains the project file
 */
exports.getGenieVersionFromProj = function (sourceLocation) {
  var genieVersion = "";
  var progFile = helper.getProjectFile(sourceLocation);
  if (progFile == null || progFile == "") return "";
  if (fs.existsSync(path.join(sourceLocation, progFile))) {
    var fileData = fs.readFileSync(path.join(sourceLocation, progFile));
    var regex = new RegExp('^.*GENIE VERSION:.*$', 'm');
    var matches = regex.exec(fileData);
    genieVersion = matches != null && matches.length > 0 ?
      matches[0].trim().replace("GENIE VERSION:", "").trim() :
      "";
  }

  return genieVersion;
}

/**
 * Add everything from a folder to TFS
 * @param  {string} sourceLocation - folder location that contains the files to add to TFS
 */
exports.addNewFolderToTFS = function (sourceLocation) {
  glob("**/*.*", { cwd: sourceLocation }, function (er, files) {
    for (var i = 0; i < files.length; i++) {
      var fullpath = path.join(sourceLocation, files[i]);
      helper.addfile(fullpath);
    }

    Promise.all([tfs.add(folders), tfs.add(newFiles)]).then(function (resolve) {
      helper.log("Done adding files to TFS")
    });
  });

}

/** 
 * do the TFS Operations
 */
exports.doTfsOperations = function () {
  var t = helper.getProjectFile("");
  return Promise.all([tfs.add(folders), tfs.add(newFiles)]);
};

/** 
 * Gets the current package version of the generators 
 */
exports.currentVersion = function () {
  return pjson.version;
};

/**
 * returns the current time
 */
exports.getCurrentTime = function() {
  return moment().format("YYYY-MM-DD, hh:mm A");
}
