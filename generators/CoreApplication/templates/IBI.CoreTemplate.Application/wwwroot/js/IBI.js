if (typeof String.prototype.startsWith !== 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) === str;
    };
}

if (typeof String.prototype.endsWith !== 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) === str;
    };
}

$.fn.removeClassPrefix = function (prefix) {
    this.each(function (i, el) {
        var classes = el.className.split(" ").filter(function (c) {
            return c.lastIndexOf(prefix, 0) !== 0;
        });
        el.className = classes.join(" ");
    });
    return this;
};

var IBI = (function () {
    var pleaseWaitDiv = null;
    var modalString = "<div class=\"modal fade\" data-backdrop=\"static\" data-keyboard=\"false\"><div class=\"processing-modal modal-content\">";
    modalString += "<div class=\"modal-header text-center\"><h4 class=\"modal-title\">|TITLE|</h4></div>";
    modalString += "<div class=\"modal-body text-center\"><img src=\"\\images\\left_loading.gif\" style=\"margin-top:50px;\"><img src=\"\\images\\right_loading.gif\" style=\"margin-top:50px;\"></div>";
    modalString += "</div></div>";
    var errorString = "<div class=\"modal fade\"><div class=\"modal-dialog\"><div class=\"modal-content\">";
    errorString += "<div class=\"modal-header\"><button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><h4 class=\"modal-title\">|TITLE|</h4></div>";
    errorString += "<div class=\"modal-body\"><div class=\"row v-center\"><div class=\"col-xs-3\"><h1><span class=\"label label-danger\"><i class=\"fa fa-exclamation-triangle\"></i></span></h1></div><div class=\"col-xs-9\"><label>|ERROR|</label></div></div></div>";
    errorString += "<div class=\"modal-footer\"><button type=\"button\" class=\"btn btn-primary\" data-dismiss=\"modal\">Ok</button></div>";
    errorString += "</div></div></div>";
    var infoString = "<div class=\"modal fade\"><div class=\"modal-dialog\"><div class=\"modal-content\">";
    infoString += "<div class=\"modal-header\"><button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><h4 class=\"modal-title\">|TITLE|</h4></div>";
    infoString += "<div class=\"modal-body\"><div class=\"row v-center\"><div class=\"col-xs-3\"><h1><span class=\"label label-warning\"><i class=\"fa fa-info-circle\"></i></span></h1></div><div class=\"col-xs-9\"><label>|MESSAGE|</label></div></div></div>";
    infoString += "<div class=\"modal-footer\"><button type=\"button\" class=\"btn btn-primary\" data-dismiss=\"modal\">Ok</button></div>";
    infoString += "</div></div></div>";

    return {
        /**
         * Helper to handle impersonation
         */
        Impersonate: (function () {
            return {
                UpdateImpersonation: function (payeeId) {
                    /// <summary>
                    /// Sends a message to the main window to update the users impersonation data
                    /// </summary>
                    window.parent.postMessage({ Event: "UpdateImpersonation", NewPayeeId: payeeId }, "*");
                }
            };
        })(),
        /**
         * Helper to deal with communication to the shell
         */
        External: (function () {
            return {
                SendUrl: function (title, relativePath) {
                    /// <summary>
                    /// Send the URL update to the main window
                    /// </summary>
                    /// <param name="title">The title of the window</param>
                    /// <param name="relativePath">The new realative path to add to the url history</param>
                    window.parent.postMessage({ Event: "NewUrl", RelativePath: relativePath, NewTitle: title }, "*");
                },
                SendFullUrl: function (title, fullPath) {
                    /// <summary>
                    /// Send the full url update to the main window
                    /// </summary>
                    /// <param name="title">The title of the window</param>
                    /// <param name="fullPath">The new full url path to add to the url history</param>
                    window.parent.postMessage({ Event: "NewUrl", FullUrl: fullPath, NewTitle: title }, "*");
                },
                HiddenMenu: function () {
                    /// <summary>
                    /// Send the full url update to the main window
                    /// </summary>
                    /// <param name="title">The title of the window</param>
                    /// <param name="fullPath">The new full url path to add to the url history</param>
                    window.parent.postMessage({ Event: "HiddenMenu" }, "*");
                },
                Unauthorize: function () {
                    /// <summary>
                    /// Send an unauthorize message to the main window
                    /// </summary>
                    window.parent.postMessage({ Event: "Unauthorize" }, "*");
                },
                Redirect: function (url) {
                    /// <summary>
                    /// Tell the main window to redirect to a new url
                    /// </summary>
                    /// <param name="url">The url to redirect to</param>
                    window.parent.postMessage({ Event: "Redirect", NewUrl: url }, "*");
                },
                YesNoBox: function (title, message) {
                    /// <summary>
                    /// Display a yes or no message box
                    /// </summary>
                    /// <param name="title">The title of the message</param>
                    /// <param name="message">The actual message itself</param>
                    window.parent.postMessage({ Event: "YesNoBox", Title: title, Message: message }, "*");
                },
                MessageBox: function (title, message) {
                    /// <summary>
                    /// Display's a message box with just an ok button
                    /// </summary>
                    /// <param name="title">The title of the message</param>
                    /// <param name="message">The message</param>
                    window.parent.postMessage({ Event: "MessageBox", Title: title, Message: message }, "*");
                },
                ShowPleaseWait: function () {
                    /// <summary>
                    /// Sends a message to the main window to show the please wait message
                    /// </summary>
                    window.parent.postMessage({ Event: "ShowPleaseWait" }, "*");
                },
                HidePleaseWait: function () {
                    /// <summary>
                    /// Sends message to hide the please wait window
                    /// </summary>
                    window.parent.postMessage({ Event: "HidePleaseWait" }, "*");
                },
                Ready: function () {
                    /// <summary>
                    /// Sends message to hide the please wait window
                    /// </summary>
                    window.parent.postMessage({ Event: "Ready" }, "*");
                }
            };
        })(),
        /**
         * Helper functions to deal with Dates
         */
        Dates: (function () {
            return {
                FromESTToCurrentUserTime: function (datestring) {
                    /// <summary>
                    /// Takes in a date string that is in the eastern time zone and converts
                    /// the string to the time zone the user is in via the UI
                    /// </summary>
                    /// <param name="datestring" type="type">The date time string</param>
                    /// <returns type="">The date time string converted to the correct time zone</returns>
                    var estTimeZoneDateTime = moment(datestring).format("YYYY-MM-DDTHH:mm:00-05:00");
                    return moment(estTimeZoneDateTime).format("MM/DD/YYYY hh:mm A");
                },
                GetNowString: function () {
                    return moment().format("MM/DD/YYYY hh:mm A");
                },
                ToString: function (value) {
                    return moment(value).format("MM/DD/YYYY hh:mm A");
                },
                FromUTCToCurrentUserTime: function (datestring) {
                    /// <summary>
                    /// Takes in a date string that is in the utc time zone and converts
                    /// the string to the time zone the user is in via the UI
                    /// </summary>
                    /// <param name="datestring" type="type">The date time string</param>
                    /// <returns type="">The date time string converted to the correct time zone</returns>
                    var utcTimeZoneDateTime = moment(datestring).format("YYYY-MM-DDTHH:mm:00-00:00");
                    return moment(utcTimeZoneDateTime).format("MM/DD/YYYY hh:mm A");
                }
            };
        })(),
        /**
         * Helper functions with the html forms
         */
        Reports: (function () {
            return {
                ClearCache: function () {
                    /// <summary>
                    /// Clears the cache of MyReports
                    /// </summary>
                    for (var i = 0, len = localStorage.length; i < len; ++i) {
                        var key = localStorage.key(i);
                        if (IBI.Utils.IsNotNull(key)) {
                            if (key.startsWith("MyReports")) {
                                localStorage.removeItem(key);
                            }
                        }
                    }
                }
            };
        })(),
        /**
         * Helper functions with the html forms
         */
        Form: (function () {
            return {
                IsSuccess: function (data) {
                    /// <summary>
                    /// Takes the data from a Successful Result and returns true
                    /// if the data is successful
                    /// </summary>
                    /// <param name="data" type="type">The data from a SuccesfulResult</param>
                    /// <returns type="">boolean</returns>
                    return IBI.Utils.IsNotNull(data.IsSuccessful) && data.IsSuccessful === true;
                },
                GetSuccessfulObject: function (data) {
                    /// <summary>
                    /// Returns the object that is contained in a successful
                    /// result
                    /// </summary>
                    /// <param name="data" type="type"></param>
                    /// <returns type=""></returns>
                    return IBI.Utils.IsNotNull(data.OutputData) ? data.OutputData : null;
                },
                ValidateHiddenFields: function () {
                    /// <summary>
                    /// Sets up the validator to allow validation of the hidden elements
                    /// </summary>
                    $.validator.setDefaults({ ignore: [] });
                },
                SetupValidation: function () {
                    /// <summary>
                    /// Re-parses the DOM to allow validation after the page load
                    /// </summary>
                    $.validator.unobtrusive.parse();
                    $.validator.unobtrusive.parse(document);
                },
                IsFormValid: function (formId, otherTestingFunction) {
                    /// <summary>
                    /// Returns true if the form passed validation
                    /// </summary>
                    /// <param name="formId" type="type">The id of the form with or without the starting #</param>
                    /// <returns type="">True if the form is valid, false if not</returns>
                    var cleanId = IBI.UI.GetJqueryId(formId);
                    var otherFunctionReturn = true;

                    if (otherTestingFunction !== undefined) {
                        otherFunctionReturn = otherTestingFunction.call();
                    }

                    return $(cleanId).valid() && otherFunctionReturn;
                },
                PostMultiPartForm: function (formId, successFunction, errorFunction) {
                    /// <summary>
                    /// Used to post a multipart form to an action
                    /// Multipart form are used when posting files becomes necessary
                    /// </summary>
                    /// <param name="formId" type="type">The id of the form with or without the starting #</param>
                    /// <param name="successFunction" type="type">The function to call if the call was a success</param>
                    /// <param name="errorFunction" type="type">The function to call if the post error-ed</param>
                    var cleanId = IBI.UI.GetJqueryId(formId);
                    var dataString;
                    var action = $(cleanId).attr("action");
                    if ($(cleanId).attr("enctype") === "multipart/form-data") {
                        dataString = new FormData($(cleanId).get(0));
                    }

                    $.ajax({
                        type: "POST",
                        url: action,
                        data: dataString,
                        contentType: false,
                        processData: false,
                        success: successFunction,
                        error: errorFunction
                    });
                },
                PostToActionWithJsonObject: function (url, jsonObject, successFunction) {
                    /// <summary>
                    /// Will post to an action with a JSON object, and will
                    /// stringify the object for you.  On success will call
                    /// the successFunction passed in
                    /// </summary>
                    /// <param name="url" type="type">The URL to the controller action</param>
                    /// <param name="jsonObject" type="type">The json object to pass into the action</param>
                    /// <param name="successFunction" type="type">The function to call on success</param>
                    $.ajax({
                        type: "POST",
                        url: url,
                        contentType: 'application/json; charset=utf-8',
                        dataType: "json",
                        data: JSON.stringify(jsonObject),
                        success: successFunction,
                        traditional: true
                    });
                },
                GetFromAction: function (area, controller, action, parameters, succesfulfunction) {
                    /// <summary>
                    /// Creates a URL based off the area controller action and the parameters
                    /// and on success calls the success function passed in
                    /// </summary>
                    /// <param name="area">Name of the area</param>
                    /// <param name="controller">Name of the controller</param>
                    /// <param name="action">Name of the action within the controller</param>
                    /// <param name="parameters">The URL parameters</param>
                    /// <param name="succesfulfunction" type="type">The function to call on the get call success</param>
                    var url = IBI.Utils.CreateMVCUrl(area, controller, action, parameters);
                    $.get(url, succesfulfunction);
                },
                ValidateSession: function (strAjaxOutput, strStatus, objXhr) {
                    /// <summary>
                    /// Used to validate if the session is still valid when
                    /// an ajax call is complete
                    /// </summary>
                    /// <param name="strAjaxOutput" type="string">Output of the ajax call</param>
                    /// <param name="strStatus" type="string">The status of the ajax call</param>
                    /// <param name="objXhr" type="object">The object of an ajax call</param>
                    /// <returns type="boolean">Will redirect the user if the session has been logged out</returns>
                    if ("undefined" !== typeof strStatus && "undefined" !== typeof objXhr) {
                        strAjaxOutput = objXhr.responseText;
                    }
                    var reg = /::SESSION EXPIRED::/;
                    if (reg.test(strAjaxOutput)) {
                        window.location = "/Account/Login?message=" + encodeURIComponent("Your session has expired please log back in to continue") + "&ReturnUrl=" + encodeURIComponent(window.location.pathname); //redirect to login page
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            };
        })(),
        /**
         * Different utilities that can be used
         */
        Utils: (function () {
            return {
                CreateStaticContentUrl: function (area, contentpath) {
                    /// <summary>
                    /// Creates a URL for static content
                    /// </summary>
                    /// <param name="area">The Name of the plugin</param>
                    /// <param name="contentpath">The path of the content after the area</param>
                    if (contentpath.startsWith("~")) contentpath = contentpath.substring(1);
                    if (contentpath.startsWith("/")) contentpath = contentpath.substring(1);
                    return "/Content/PluginContent/" + area + "/" + contentpath;
                },
                CreateUrlParameter: function (name, value) {
                    /// <summary>
                    /// Creates a parameter for the create url functions
                    /// </summary>
                    /// <param name="name">Name of the parameter</param>
                    /// <param name="value">Value of the parameter</param>
                    return { Name: name, Value: value };
                },
                CreateMVCUrl: function CreateMVCUrl(area, controller, action, parameters) {
                    /// <summary>
                    /// Creates an MVC based URL for an area, controller, action with
                    /// named based parameters
                    ///
                    /// Examples of the parameters
                    /// [{ Name: "id", Value: value }, { Name: "text", Value: text-value}]
                    /// </summary>
                    /// <param name="area">Name of the area</param>
                    /// <param name="controller">Name of the controller</param>
                    /// <param name="action">Name of the action within the controller</param>
                    /// <param name="parameters">The URL parameters</param>
                    var paramString = "";
                    var actionString = action.toLowerCase() === "index" ? "" : action;
                    if (parameters !== null) {
                        for (var i = 0; i < parameters.length; ++i) {
                            if (paramString.length > 0) paramString += "&";
                            paramString += parameters[i].Name + "=" + parameters[i].Value;
                        }
                    }
                    if (paramString.length > 0) paramString = "?" + paramString;
                    return "/" + (area !== "" ? area + "/" : "") + controller + (actionString.length > 0 ? "/" + actionString + "/" : "") + paramString;
                },
                CreateIdMVCUrl: function (area, controller, action, value) {
                    /// <summary>
                    /// Creates an MVC based URL for an area, controller, action
                    /// and sets the id parameter to the value
                    ///
                    /// </summary>
                    /// <param name="area">Name of the area</param>
                    /// <param name="controller">Name of the controller</param>
                    /// <param name="action">Name of the action within the controller</param>
                    /// <param name="value">The value of the id parameter</param>
                    var parameters = [];
                    parameters.push(this.CreateUrlParameter("id", value));
                    return this.CreateMVCUrl(area, controller, action, parameters);
                },
                Redirect: function (area, controller, action, parameters) {
                    /// <summary>
                    /// Creates an MVC based URL for an area, controller, action with
                    /// named based parameters and then redirects the user to that URL
                    ///
                    /// Examples of the parameters
                    /// [{ Name: "id", Value: value }, { Name: "text", Value: text-value}]
                    /// </summary>
                    /// <param name="area">Name of the area</param>
                    /// <param name="controller">Name of the controller</param>
                    /// <param name="action">Name of the action within the controller</param>
                    /// <param name="parameters">The URL parameters</param>
                    var url = this.CreateMVCUrl(area, controller, action, parameters);
                    window.location.href = url;
                },
                RedirectId: function (area, controller, action, value) {
                    /// <summary>
                    /// Creates an MVC based URL for an area, controller, action
                    /// and sets the id parameter to the value, and then redirects
                    /// the user to that new URL
                    /// </summary>
                    /// <param name="area">Name of the area</param>
                    /// <param name="controller">Name of the controller</param>
                    /// <param name="action">Name of the action within the controller</param>
                    /// <param name="value">The value of the id parameter</param>
                    var url = this.CreateIdMVCUrl(area, controller, action, value);
                    window.location.href = url;
                },
                IsNotNull: function (value) {
                    /// <summary>
                    /// Returns true if the value is not null and is not undefined
                    /// </summary>
                    /// <param name="value">The value to test</param>
                    return value !== null && value !== undefined;
                },
                IsNull: function (value) {
                    /// <summary>
                    /// Returns true if the value is null or is undefined
                    /// </summary>
                    /// <param name="value">The value to test</param>
                    return value === null || value === undefined;
                },
                IsEmpty: function (value) {
                    /// <summary>
                    /// Returns true if the is an empty string
                    /// </summary>
                    /// <param name="value">The value to test</param>
                    return value === "";
                },
                IsNotEmpty: function (value) {
                    /// <summary>
                    /// Returns true if the value is not an empty string
                    /// </summary>
                    /// <param name="value">The value to test</param>
                    return value !== "";
                },
                IsInArray: function (value, array) {
                    /// <summary>
                    /// Returns true if the value is in an array
                    /// </summary>
                    /// <param name="value">The value to find</param>
                    /// <param name="array">The array to test</param>
                    return $.inArray(value, array) !== -1;
                },
                RefreshPage: function () {
                    /// <summary>
                    /// Refreshed the current page the user is on
                    /// </summary>
                    location.reload();
                },
                JavascriptDateToEnglishDate: function (value) {
                    /// <summary>
                    /// Takes a Javascript data which is a large integer and uses moment
                    /// to convert it to a readable English version of the date
                    /// </summary>
                    /// <param name="value"></param>
                    return moment(new Date(parseInt(value.substr(6)))).format("MM/DD/YYYY");
                },
                JavascriptDateToEnglishDateTime: function (value) {
                    /// <summary>
                    /// Takes a Javascript data which is a large integer and uses moment
                    /// to convert it to a readable English version of the date time
                    /// </summary>
                    /// <param name="value"></param>
                    return moment(new Date(parseInt(value.substr(6)))).format("MM/DD/YYYY h:mm:ss a");
                }
            }
        })(),
        /**
         * Different constants to help
         */
        CONST: {
            StringEmpty: ""
        },
        /**
         * Handles the bootstrap table functions
         */
        BootstrapTable: (function () {
            return {
                GetUniqueRow: function (id, uniqueValue) {
                    /// <summary>
                    /// Get a row from bootstrap table of the unique value
                    /// </summary>
                    /// <param name="id" type="string">The id of the table with or without the starting #</param>
                    /// <param name="uniqueValue" type="string">The value to retrieve</param>
                    /// <returns type="JSON"></returns>
                    return $(IBI.UI.GetJqueryId(id)).bootstrapTable('getRowByUniqueId', uniqueValue);
                },
                UpdateRow: function (id, index, row) {
                    /// <summary>
                    /// Updates a row on a bootstrap table with an update row
                    /// </summary>
                    /// <param name="id" type="string">The id of the table with or without the starting #</param>
                    /// <param name="index" type="int">The index of the row to update</param>
                    /// <param name="row" type="JSON object">The data to update</param>
                    $(IBI.UI.GetJqueryId(id)).bootstrapTable('updateRow', { index: index, row: row });
                },
                RemoveRow: function (id, fieldName, fieldValue) {
                    /// <summary>
                    /// Remove a single row from a bootstrap table
                    /// </summary>
                    /// <param name="id" type="string">The id of the table with or without the starting #</param>
                    /// <param name="fieldName" type="string">The name of the field to remove the row</param>
                    /// <param name="fieldValue" type="string">The value of the field to remove</param>
                    this.RemoveRows(id, fieldName, [fieldValue]);
                },
                RemoveRows: function (id, fieldName, arrayOfFieldValue) {
                    /// <summary>
                    /// Removes a group of row from a bootstrap table
                    /// </summary>
                    /// <param name="id" type="string">The id of the table with or without the starting #</param>
                    /// <param name="fieldName" type="string">The name of the field to remove the row</param>
                    /// <param name="arrayOfFieldValue" type="array">The array of values of the field to remove</param>
                    $(IBI.UI.GetJqueryId(id)).bootstrapTable('remove', { field: fieldName, values: arrayOfFieldValue });
                },
                GetSelections: function (id) {
                    /// <summary>
                    /// Get the selections from a bootstrap table
                    /// </summary>
                    /// <param name="id" type="string">he id of the table with or without the starting #</param>
                    /// <returns type="array"></returns>
                    return $(IBI.UI.GetJqueryId(id)).bootstrapTable('getSelections');
                },
                OnLoad: function (tableId, onloadFunction) {
                    /// <summary>
                    /// Setups of the load function for a bootstrap table on load
                    /// onloadFunction should be function(event, data)
                    /// </summary>
                    /// <param name="tableId" type="string">The id of the table with or without the starting #</param>
                    /// <param name="onloadFunction" type="function">The function to call after load</param>
                    $(IBI.UI.GetJqueryId(tableId)).on("load-success.bs.table", onloadFunction);
                },
                Refresh: function (id, urlString) {
                    /// <summary>
                    /// Refreshes a bootstrap table based on the id
                    /// Will set the URL to refresh the table with if defined
                    /// </summary>
                    /// <param name="id">The id of the bootstrap table with or without the starting #</param>
                    /// <param name="urlString">The URL to get the data from</param>
                    var correctId = IBI.UI.GetJqueryId(id);
                    if (urlString === null || urlString === undefined) {
                        $(correctId).bootstrapTable('refresh');
                    } else {
                        $(correctId).bootstrapTable('refresh', { url: urlString });
                    }
                },
                GetData: function (id) {
                    /// <summary>
                    /// Returns a JSON with the data from a bootstrap table
                    /// </summary>
                    /// <param name="id">The id of the bootstrap table with or without the starting #</param>
                    var correctId = IBI.UI.GetJqueryId(id);
                    return $(correctId).bootstrapTable('getData');
                },
                AddDataTo: function (id, data) {
                    /// <summary>
                    /// Will append data to a bootstrap table
                    /// </summary>
                    /// <param name="id" type="type">The id of the table with or without the starting #</param>
                    /// <param name="data" type="type">The data to append to the table</param>
                    $(IBI.UI.GetJqueryId(id)).bootstrapTable('append', data);
                },
                IsInCardView: function (id) {
                    /// <summary>
                    /// Returns true if the bootstrap table is in the card view currently
                    /// </summary>
                    /// <param name="id">The id of the bootstrap table with or without the starting #</param>
                    var correctId = IBI.UI.GetJqueryId(id);
                    var opts = $(correctId).bootstrapTable('getOptions');
                    return opts.cardView;
                }
            };
        })(),
        /**
         * Handles the UI functions
         */
        UI: (function () {
            return {
                EncodeString: function (data) {
                    /// <summary>
                    /// Encode the data
                    /// </summary>
                    /// <param name="data" type="type">The data which needs to be encoded</param>
                    return encodeURIComponent(data);
                },
                DecodeString: function (data) {
                    /// <summary>
                    /// Decode the data
                    /// </summary>
                    /// <param name="data" type="type">The data which needs to be decoded</param>
                    return decodeURIComponent(data);
                },
                ClearValidationErrorsOnFormChanges: function (formId) {
                    /// <summary>
                    /// Enables events on a form to correct bootstrap and jQuery validation issues
                    /// where the errors are not cleared once a user corrects the error
                    /// </summary>
                    /// <param name="formId">The id of the form with or without the starting #</param>
                    var sanitizedFormId = this.GetJqueryId(formId);
                    $(sanitizedFormId).on("keyup change propertychange", "input.input-validation-error, input.valid, textarea.input-validation-error, textarea.valid, select.input-validation-error, select.valid", function () {
                        if ($(sanitizedFormId).valid()) {
                            $(".validation-summary-errors").addClass("hidden");
                        } else {
                            $(".validation-summary-errors").removeClass("hidden");
                        }
                    });
                },
                ShowTitleWait: function (title) {
                    /// <summary>
                    /// Show a wait dialog with a title
                    /// </summary>
                    /// <param name="title">The title of the pop-up</param>
                    if (pleaseWaitDiv !== null) this.HidePleaseWait();
                    pleaseWaitDiv = null;
                    var titledModal = modalString.replace("|TITLE|", title);
                    pleaseWaitDiv = $(titledModal);
                    pleaseWaitDiv.modal();
                },
                ShowWaitDialog: function (title) {
                    /// <summary>
                    /// Show a wait dialog with a title
                    /// </summary>
                    /// <param name="title">The title of the pop-up</param>
                    this.ShowTitleWait(title);
                },
                ShowPleaseWait: function () {
                    /// <summary>
                    /// Show a wait with "Processing..." as the title
                    /// </summary>
                    this.ShowTitleWait("Processing..");
                },
                HidePleaseWait: function () {
                    /// <summary>
                    /// Hides the please wait dialog
                    /// </summary>
                    if (pleaseWaitDiv !== null) {
                        pleaseWaitDiv.modal('hide');
                    }
                },
                ShowError: function (errortext, title) {
                    /// <summary>
                    /// Shows an error bootstrap modal based on a
                    /// text message and a title
                    /// </summary>
                    /// <param name="infotext">The text of the error</param>
                    /// <param name="title">The title of the pop-up</param>
                    if (pleaseWaitDiv !== null) this.HidePleaseWait();
                    pleaseWaitDiv = null;
                    var errormodal = errorString.replace("|ERROR|", errortext).replace("|TITLE|", title);
                    pleaseWaitDiv = $(errormodal);
                    pleaseWaitDiv.modal();
                },
                ShowInfomation: function (infotext, title) {
                    /// <summary>
                    /// Shows an information bootstrap modal based on a
                    /// text message and a title
                    /// </summary>
                    /// <param name="infotext">The text of the information</param>
                    /// <param name="title">The title of the pop-up</param>
                    if (pleaseWaitDiv !== null) this.HidePleaseWait();
                    pleaseWaitDiv = null;
                    var infomodal = infoString.replace("|MESSAGE|", infotext).replace("|TITLE|", title);
                    pleaseWaitDiv = $(infomodal);
                    pleaseWaitDiv.modal();
                },
                ShowWaitDialogAndHideModal: function (title, modalId) {
                    /// <summary>
                    /// Hides a bootstrap modal and then shows the wait dialog with a title
                    /// </summary>
                    /// <param name="title">The title of the wait dialog</param>
                    /// <param name="modalId">The id of the modal to hide with or with out the starting #</param>
                    this.HideBootStrapModal(modalId);
                    this.ShowWaitDialog(title);
                },
                HideWaitDialogAndShowModal: function (modalId) {
                    /// <summary>
                    /// Hides the please wait dialog and shows the modal
                    /// </summary>
                    /// <param name="modalId" type="type">The id of the modal to hide with or with out the starting #</param>
                    this.ShowBootStrapModal(modalId);
                    this.HidePleaseWait();
                },
                GetJqueryId: function (id) {
                    /// <summary>
                    /// Get the id for jQuery based on a string
                    /// Basically ensures there is a # in front of the string
                    /// </summary>
                    /// <param name="id">The id of the element with or without the starting #</param>
                    if (id.startsWith("#")) return id;
                    else return "#" + id;
                },
                GetJqueryCSSClass: function (cssClass) {
                    /// <summary>
                    /// Ensures the css class start with a . when
                    /// manipulating any dom object by css class
                    /// </summary>
                    /// <param name="cssClass" type="type">The css class with or without the starting period</param>
                    /// <returns type="">The cleaned up css class</returns>
                    if (cssClass.startsWith(".")) return cssClass;
                    else return "." + cssClass;
                },
                GetHiddenValue: function (id) {
                    /// <summary>
                    /// Gets the value of a hidden field
                    /// </summary>
                    /// <param name="id">The id of the field with or without the starting #</param>
                    return $(this.GetJqueryId(id)).val();
                },
                SetHiddenValue: function (id, value) {
                    /// <summary>
                    /// Sets the value of a hidden field
                    /// </summary>
                    /// <param name="id"></param>
                    /// <param name="id">The id of the field with or without the starting #</param>
                    $(this.GetJqueryId(id)).val(value);
                },
                GetTextBoxValue: function (id) {
                    /// <summary>
                    /// Gets the value of a text-box
                    /// </summary>
                    /// <param name="id"></param>
                    /// <param name="id">The id of the field with or without the starting #</param>
                    return $(this.GetJqueryId(id)).val();
                },
                SetTextBoxValue: function (id, value) {
                    /// <summary>
                    /// Sets the value of a text-box
                    /// </summary>
                    /// <param name="id"></param>
                    /// <param name="id">The id of the field with or without the starting #</param>
                    /// <param name="value">The value to set on the text-box</param>
                    //add .change to trigger change on read-only text-boxes
                    $(this.GetJqueryId(id)).val(value).change();
                    $(this.GetJqueryId(id)).attr("value", value);
                },
                IsCheckBoxCheck: function (id) {
                    /// <summary>
                    /// Returns true if the check-box is checked
                    /// </summary>
                    /// <param name="id"></param>
                    /// <param name="id">The id of the check-box with or without the starting #</param>
                    return $(this.GetJqueryId(id)).prop('checked');
                },
                SetCheckBoxCheck: function (id, value) {
                    /// <summary>
                    /// Sets the check-box checked or not
                    /// </summary>
                    /// <param name="id">The id of the field with or without the starting #</param>
                    /// <param name="value">True/False to check or unchecked a check-box</param>
                    $(this.GetJqueryId(id)).prop('checked', value);
                },
                GetDropDownValue: function (id) {
                    /// <summary>
                    /// Gets the selected value from a dropdown
                    /// </summary>
                    /// <param name="id">The id of the dropdown with or without the starting #</param>
                    return $(this.GetJqueryId(id)).val();
                },
                SetDropDown: function (id, value) {
                    /// <summary>
                    /// Sets the selected value for a drop down list
                    /// </summary>
                    /// <param name="id">The id of the dropdown with or without the starting #</param>
                    /// <param name="value">The value to set as the selected value</param>
                    $(this.GetJqueryId(id)).val(value);
                },
                SetDropDownValueByText: function (id, text) {
                    /// <summary>
                    /// Sets a dropdown option as select by the text and not
                    /// the value of the dropdown
                    /// </summary>
                    /// <param name="id" type="type">The id of the dropdown with or without the starting #</param>
                    /// <param name="text" type="string">The text to set as selected</param>
                    $(this.GetJqueryId(id) + " option:contains(" + text + ")").attr('selected', 'selected');
                },
                ClearDropDownList: function (id) {
                    /// <summary>
                    /// Clear the drop down list
                    /// </summary>
                    /// <param name="id">The id of the dropdown with or without the starting #</param>
                    $(this.GetJqueryId(id)).empty();
                },
                AddOptionToDropdown: function (id, value, text) {
                    /// <summary>
                    /// Adds an option to a dropdown setting the value and text of the new option
                    /// </summary>
                    /// <param name="id">The id of the dropdown with or without the starting #</param>
                    /// <param name="value">The value of the selection</param>
                    /// <param name="text">The text that appears on screen on the dropdown</param>
                    $(this.GetJqueryId(id)).append($("<option></option>").attr("value", value).text(text));
                },
                GetDropDownSelectedText: function (id) {
                    /// <summary>
                    /// Get the selected text of the selected value from a text-box
                    /// </summary>
                    /// <param name="id">The id of the dropdown with or without the starting #</param>
                    return $(this.GetJqueryId(id) + " option:selected").text();
                },
                GetRadioSelectedValue: function (name) {
                    /// <summary>
                    /// Get the selected value from a radio button list
                    /// </summary>
                    /// <param name="id">The id of the radio button list with or without the starting #</param>
                    var selectedVal = "";
                    var selected = $("input[type='radio'][name='" + name + "']:checked");
                    if (selected.length > 0) { selectedVal = selected.val(); }
                    return selectedVal;
                },
                SetLabelText: function (id, value) {
                    /// <summary>
                    /// Sets the text of a label
                    /// </summary>
                    /// <param name="id">The id of the label with or without the starting #</param>
                    /// <param name="value">The value to set as text on the label</param>
                    $(this.GetJqueryId(id)).html(value);
                },
                GetLabelText: function (id) {
                    /// <summary>
                    /// Get the text of a label
                    /// </summary>
                    /// <param name="id">The id of the label with or without the starting #</param>
                    return $(this.GetJqueryId(id)).html();
                },
                ShowBootStrapModal: function (id) {
                    /// <summary>
                    /// Shows a bootstrap modal
                    /// </summary>
                    /// <param name="id">The id of the bootstrap modal with or without the starting #</param>
                    $(this.GetJqueryId(id)).modal('show');
                },
                HideBootStrapModal: function (id) {
                    /// <summary>
                    /// Hides a bootstrap modal
                    /// </summary>
                    /// <param name="id">The id of the bootstrap modal with or without the starting #</param>
                    $(this.GetJqueryId(id)).modal('hide');
                },
                ShowItem: function (id) {
                    /// <summary>
                    /// Removes the hidden class from a HTML element
                    /// </summary>
                    /// <param name="id">The id of the element with or without the starting #</param>
                    var correctId = this.GetJqueryId(id);
                    $(correctId).removeClass("hidden");
                },
                HideItem: function (id) {
                    /// <summary>
                    /// Adds the hidden class and removes the show class
                    /// to a HTML element
                    /// </summary>
                    /// <param name="id">The id of the element with or without the starting #</param>
                    var correctId = this.GetJqueryId(id);
                    $(correctId).addClass("hidden");
                    $(correctId).removeClass("show");
                },
                ShowItemByClass: function (className) {
                    /// <summary>
                    /// Takes a CSS class and adds the
                    /// show css class and remove the hidden
                    /// css class
                    /// </summary>
                    /// <param name="className" type="type">The css class name to show</param>
                    if (!className.startsWith(".")) {
                        className = "." + className;
                    }
                    $(className).removeClass("hidden");
                },
                HideItemByClass: function (className) {
                    /// <summary>
                    /// Takes a CSS class and adds the
                    /// hidden css class and removes the show
                    /// css class
                    /// </summary>
                    /// <param name="className" type="type">The css class name to hide</param>
                    if (!className.startsWith(".")) {
                        className = "." + className;
                    }
                    $(className).removeClass("show");
                    $(className).addClass("hidden");
                },
                RefreshBootstrapTable: function (id, urlString) {
                    /// <summary>
                    /// Refreshes a bootstrap table based on the id
                    /// Will set the URL to refresh the table with if defined
                    /// </summary>
                    /// <param name="id">The id of the bootstrap table with or without the starting #</param>
                    /// <param name="urlString">The URL to get the data from</param>
                    IBI.BootstrapTable.Refresh(id, urlString);
                },
                GetBootstrapTableData: function (id) {
                    /// <summary>
                    /// Returns a JSON with the data from a bootstrap table
                    /// </summary>
                    /// <param name="id">The id of the bootstrap table with or without the starting #</param>
                    return IBI.BootstrapTable.GetData(id);
                },
                IsBootstrapTableInCardView: function (id) {
                    /// <summary>
                    /// Returns true if the bootstrap table is in the card view currently
                    /// </summary>
                    /// <param name="id">The id of the bootstrap table with or without the starting #</param>
                    return IBI.BootstrapTable.IsInCardView(id);
                },
                SetHtml: function (id, data) {
                    /// <summary>
                    /// Takes an id of a dom object and sets the HTML of that id
                    /// </summary>
                    /// <param name="id" type="type">The id of the dom object with or without the starting #</param>
                    /// <param name="data" type="type">The data to set</param>
                    var correctId = this.GetJqueryId(id);
                    $(correctId).html(data);
                },
                ClearHtml: function (id) {
                    /// <summary>
                    /// Clears out the inner html of the dom object with this id
                    /// </summary>
                    /// <param name="id" type="type">The id of the dom object with or without the starting #</param>
                    var correctId = this.GetJqueryId(id);
                    $(correctId).empty();
                },
                AddDataToBootstrapTable: function (id, data) {
                    /// <summary>
                    /// Will append data to a bootstrap table
                    /// </summary>
                    /// <param name="id" type="type">The id of the table with or without the starting #</param>
                    /// <param name="data" type="type">The data to append to the table</param>
                    IBI.BootstrapTable.AddDataTo(id, data);
                },
                AppentHtmlToElement: function (id, htmlData) {
                    /// <summary>
                    /// Appends html to an element by the element's Id
                    /// </summary>
                    /// <param name="id" type="string">The id of the element with or without the starting #</param>
                    /// <param name="htmlData" type="string">The html data</param>
                    $(this.GetJqueryId(id)).append(htmlData);
                }
            };
        })(),
        /**
         * Handles the local/session Storage
         */
        Storage: (function () {
            return {
                /**
                 * Creates a key name based on the area and key name you would like to use
                 * Example:  Area is PipelineCRM and the key name is AllStatus the
                 * new key name will be PipelineCRM-AllStatus
                 * @param {string} area - The id the textbox (with or without the starting #)
                 * @param {string} name - The id the textbox (with or without the starting #)
                 * @returns {string} - the key that is in the local storage
                 */
                GetKeyName: function (area, name) {
                    var keyname = IBI.Utils.IsNotNull(area) && IBI.Utils.IsNotEmpty(area) ? area + "-" : "";
                    return keyname + name;
                },
                /**
                 * Set data to the Session Storage
                 * @param {string} area - The name of the area
                 * @param {string} keyname - The name of the key
                 * @param {type} objectData - The id the textbox (with or without the starting #)
                 */
                SetSessionData: function (area, keyname, objectData) {
                    var sessionKeyName = this.GetKeyName(area, keyname);
                    sessionStorage.setItem(sessionKeyName, JSON.stringify(objectData));
                },
                /**
                 * Set data to the Local Storage
                 * @param {string} area - The name of the area
                 * @param {string} keyname - The name of the key
                 * @param {type} objectData - The object to add to the local storage
                 */
                SetLocalData: function (area, keyname, objectData) {
                    /// <summary>
                    /// Set data to the Local Storage
                    /// </summary>
                    /// <param name="area" type="string">The name of the area</param>
                    /// <param name="keyname" type="string">The name of the key</param>
                    /// <param name="objectData" type="object">The object to add to the local storage</param>
                    var sessionKeyName = this.GetKeyName(area, keyname);
                    localStorage.setItem(sessionKeyName, JSON.stringify(objectData));
                },
                /**
                 * Get data from the session storage
                 * @param {string} area - The name of the area
                 * @param {string} keyname - The name of the key
                 * @returns {object} - the object placed in the sesstion storage
                 */
                GetSessionData: function (area, keyname) {
                    return JSON.parse(sessionStorage.getItem(this.GetKeyName(area, keyname)));
                },
                /**
                 * Get data from the local storage
                 * @param {string} area - The name of the area
                 * @param {string} keyname - The name of the key
                 * @returns {object} - the object placed in the local storage
                 */
                GetLocalData: function (area, keyname) {
                    return JSON.parse(localStorage.getItem(this.GetKeyName(area, keyname)));
                }
            };
        })(),
        /**
         * Handles Autocomplete textboxes
         */
        Tokenizer: (function () {
            return {
                Setup: function (textboxId, dataUrl, typeAheadValue, typeAheadLabel) {
                    var token = [];
                    $.getJSON(dataUrl, function (response) {
                        $.each(response, function (i, v) {
                            token.push({ value: v[typeAheadValue], label: v[typeAheadLabel] });
                        });
                        var tagEngine = new Bloodhound({
                            local: token,
                            datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.label); },
                            queryTokenizer: Bloodhound.tokenizers.whitespace
                        });
                        tagEngine.initialize();
                        $(IBI.UI.GetJqueryId(textboxId)).tokenfield({
                            typeahead: [null, {
                                name: 'tagEngine',
                                displayKey: 'label',
                                source: tagEngine.ttAdapter()
                            }]
                        })
                            .on('tokenfield:createtoken', function (event) {
                                var existingTokens = $(this).tokenfield('getTokens');
                                $.each(existingTokens, function (index, token) {
                                    if (token.value === event.attrs.value) { event.preventDefault(); }
                                });
                            });
                    });
                }
            };
        })(),
        AutoComplete: (function () {
            return {
                /**
                 * @param {string} textBoxId - The id the textbox (with or without the starting #)
                 * @param {string} hiddenFieldId - The id the textbox (with or without the starting #)
                 * @param {string} dataUrl - The url to get the autocomplete
                 * @param {function} searchValueFunction - the javascript function to get the search type
                 */
                Setup: function (textBoxId, hiddenFieldId, dataUrl, searchValueFunction) {
                    $(IBI.UI.GetJqueryId(textBoxId)).autocomplete({
                        minLength: 2,
                        select: function (event, ui) {
                            IBI.UI.SetHiddenValue(hiddenFieldId, ui.item.id);
                            return true;
                        },
                        source: function (request, response) {
                            var term = encodeURIComponent(request.term);
                            IBI.UI.SetHiddenValue(hiddenFieldId, "");
                            var searchType = "";
                            if (searchValueFunction !== undefined) {
                                searchType = searchValueFunction.call();
                            }
                            $.getJSON(dataUrl + "?term=" + term + "&searchType=" + searchType, request, function (data, status, xhr) {
                                response(data._options);
                            });
                        }
                    }).data('ui-autocomplete')._renderItem = function (ul, item) {
                        return $('<li>')
                            .data('ui-autocomplete-item', item)
                            .append('<hr /><a>' + item.label + '</a>')
                            .appendTo(ul);
                    };
                }
            };
        })(),
        /**
         * Handles <%= Name %>
         */
        <%= Name %>: (function () {
            return {
                CreateStaticContentUrl: function (contentpath) {
                    /// <summary>
                    /// Create a static content path for <%= Name %>
                    /// </summary>
                    /// <param name="contentpath" type="string"></param>
                    /// <returns type=""></returns>
                    return IBI.Utils.CreateStaticContentUrl("", contentpath);
                },
                Create<%= Name %>Url: function (controller, action, parameters) {
                    /// <summary>
                    /// Create an URL for the <%= Name %> area for a controller,
                    /// action and an array of parameters
                    /// </summary>
                    /// <param name="controller" type="string">The controller</param>
                    /// <param name="action" type="string">The name of the action</param>
                    /// <param name="parameters" type="array">Array of parameters</param>
                    /// <returns type="string">The URL</returns>
                    return IBI.Utils.CreateMVCUrl("", controller, action, parameters);
                },
                CreateId<%= Name %>Url: function (controller, action, id) {
                    /// <summary>
                    /// Create an URL for the <%= Name %> that passes
                    /// in a parameter by the name of id
                    /// </summary>
                    /// <param name="controller" type="string">The controller</param>
                    /// <param name="action" type="string">The name of the action</param>
                    /// <param name="id" type="int">The value to pass into the Id parameter</param>
                    /// <returns type="string">The URL</returns>
                    return IBI.Utils.CreateIdMVCUrl("", controller, action, id);
                }
            };
        })()
    };
})();