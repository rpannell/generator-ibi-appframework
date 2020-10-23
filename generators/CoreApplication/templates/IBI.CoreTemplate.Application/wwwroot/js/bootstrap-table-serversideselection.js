/**
 * Used to have a selection column (be it checkbox or radio) and deal with
 * server side pagination at the same time.  The developer will need to set the
 * option "serverSideSelection" to true and set at least the "idField" option or the
 * "serverSideSelectionId" to a unique key in the data model to track what has been
 * selected.
 *
 * The option "serverSideCheckboxProperty" can be set to the property name in the
 * model that has the boolean value in the data model that says if the data is
 * selected or not.  By default it will add "serversideselectionstate" in the model
 * if this option isn't populated
 *
 * The data-field in the column that contains the checkbox or radio buttom must contain
 * the same string as what's in the "serverSideCheckboxProperty"
 *
 * @author: Rodney Pannell
 * @version: v1.0.0
 */
(function ($) {
    'use strict';

    /**
     * Helper function to make sure a string has a value
     * @param {string} str
     */
    var hasValue = function (str) {
        return str != null && str != undefined && str != ""
    };

    /**
     * Check to see if we should track the changes
     * @param {object} options
     */
    var hasServerSideSelection = function (options) {
        if (options.serverSideSelection && options.data.filter(x => x[options.serverSideSelectionId] != undefined).length == 0)
            console.warn("The property " + options.serverSideSelectionId + " doesn't exists in the model");
        return options.data.filter(x => x[options.serverSideSelectionId] != undefined).length > 0 &&
            options.serverSideSelection &&
            hasValue(options.serverSideSelectionId) &&
            hasValue(options.serverSideCheckboxProperty);
    }

    /**
     * set the selections when an event is triggered
     * @param {object} options the bootstrap table options object
     */
    var setSelections = function (options) {
        //only check tracking if there is data to track
        if (hasServerSideSelection(options)) {
            var checkboxId = options.serverSideCheckboxProperty;
            var fieldName = options.serverSideSelectionId;
            $.each(options.data, function (i, row) {
                if (row[checkboxId]) {
                    if (currentSelections.filter(sel => sel[fieldName] == row[fieldName]).length == 0) {
                        currentSelections.push(row);
                    }
                } else {
                    currentSelections = currentSelections.filter(sel => sel[fieldName] != row[fieldName]);
                }
            });
        }
    };

    /**
     * Add new options to the bootstrap table
     */
    $.extend($.fn.bootstrapTable.defaults, {
        serverSideSelectionId: "",
        serverSideCheckboxProperty: "serversideselectionstate",
        serverSideSelection: false
    });

    /**
     * Add new methods to the bootstrap table to get the server side selections
     */
    $.fn.bootstrapTable.methods.push('getServerSideSelections');

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _init = BootstrapTable.prototype.init,
        _onCheck = BootstrapTable.DEFAULTS.onCheck,
        _onUnCheck = BootstrapTable.DEFAULTS.onUncheck,
        _onUnCheckAll = BootstrapTable.DEFAULTS.onUncheckAll,
        _onCheckAll = BootstrapTable.DEFAULTS.onCheckAll,
        _onCheckSome = BootstrapTable.DEFAULTS.onCheckSome,
        _onUncheckSome = BootstrapTable.DEFAULTS.onUncheckSome,
        _getData = BootstrapTable.prototype.getData,
        _responseHandler = BootstrapTable.DEFAULTS.responseHandler,
        currentSelections = [];

    /**
     * Override the response handler function to call our own function to set
     * the server side selections
     */
    BootstrapTable.DEFAULTS.responseHandler = function (res) {
        var that = this;
        res = _responseHandler.apply(this, Array.prototype.slice.apply(arguments));
        //if we are persisting the checkbox call this function
        if (that.serverSideSelection) {
            $.each(res.rows, function (i, row) {
                if (row[that.serverSideCheckboxProperty] == undefined) row[that.serverSideCheckboxProperty] = false;
                row[that.serverSideCheckboxProperty] = currentSelections.filter(sel => sel[that.serverSideSelectionId] == row[that.serverSideSelectionId]).length > 0 ? true : false;
            });
        }
        return res;
    };

    /**
     * Return the server side selections the user has selected
     */
    BootstrapTable.prototype.getServerSideSelections = function () {
        return currentSelections == undefined || currentSelections == null ? [] : currentSelections;
    }

    /**
     * Called on the "check" event
     */
    BootstrapTable.DEFAULTS.onCheck = function (row) {
        _onCheck.apply(this, Array.prototype.slice.apply(arguments));
        setSelections(this);
    };

    /**
     * Called on the "uncheck.bs.table" event
     */
    BootstrapTable.DEFAULTS.onUncheck = function (row) {
        _onUnCheck.apply(this, Array.prototype.slice.apply(arguments));
        setSelections(this);
    };

    /**
     * Called on the "check-all.bs.table" event
     */
    BootstrapTable.DEFAULTS.onCheckAll = function (rows) {
        _onCheckAll.apply(this, Array.prototype.slice.apply(arguments));
        setSelections(this);
    };

    /**
     * Called on the "uncheck-all.bs.table" event
     */
    BootstrapTable.DEFAULTS.onUncheckAll = function (rows) {
        _onUnCheckAll.apply(this, Array.prototype.slice.apply(arguments));
        setSelections(this);
    };

    /**
     * Called on the "check-some.bs.table" event
     */
    BootstrapTable.DEFAULTS.onCheckSome = function (rows) {
        _onCheckSome.apply(this, Array.prototype.slice.apply(arguments));
        setSelections(this);
    };

    /**
     * Called on the "uncheck-some.bs.table" event
     */
    BootstrapTable.DEFAULTS.onUncheckSome = function (rows) {
        _onUncheckSome.apply(this, Array.prototype.slice.apply(arguments));
        setSelections(this);
    };

    /**
     * Make sure the extension is setup correctly
     */
    BootstrapTable.prototype.init = function () {
        //check if the server side selection is turned on
        if (this.options.serverSideSelection) {
            //set the primary key to the id-field if it's present and not the primary key
            if (!hasValue(this.options.serverSideSelectionId)) {
                if (hasValue(this.options.idField)) {
                    this.options.serverSideSelectionId = this.options.idField;
                }
            }

            //make sure everything is set, if not turn it off
            if (!hasValue(this.options.serverSideSelectionId) || !hasValue(this.options.serverSideCheckboxProperty)) {
                this.options.serverSideSelection = false;
                console.warn("Developer needs to set both the server-side-selection-id (or id-field) and the server-side-checkbox-property, turning off server side selections");
            }
        }
        _init.apply(this, Array.prototype.slice.apply(arguments));
    }
})(jQuery);