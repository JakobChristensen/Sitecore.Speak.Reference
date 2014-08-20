import $ = require("jquery");
import Speak = require("sitecore/shell/client/Speak/Assets/lib/core/1.2/SitecoreSpeak");

declare var typeaheaddata;

class FiddlePageCode extends Speak.PageCode {
  runButton: JQuery = $("#runButton");
  saveButton: JQuery = $("#saveButton");
  forkButton: JQuery = $("#forkButton");
  uploadForm: JQuery = $("#uploadForm");
  uploadInput: JQuery = $("#uploadInput");
  uploadFileName: JQuery = $("#uploadFileName");
  uploadButton: JQuery = $("#uploadButton");
  codeHelp: JQuery = $("#codehelp");
  codeHelpText: JQuery = $("#codehelptext");
  output: JQuery = $("#output");
  outputPanel: JQuery = $("#outputpanel");
  action: JQuery = $("#action");
  form: JQuery = $("#fiddleForm");
  textAreas: JQuery = $("textarea");
  iframes: JQuery = $("iframe");
  window: JQuery = $(window);
  readOnly: JQuery = $("#readonly");

  initialize() {
    if (this.readOnly.val() == "True") {
      this.saveButton.hide();
    }

    this.runButton.on("click", () => this.runFiddle());
    this.saveButton.on("click", () => this.saveFiddle());
    this.forkButton.on("click", () => this.forkFiddle());
    this.codeHelp.on("change", (evt) => this.displayHelp(evt));
    this.uploadInput.on("change", () => this.uploadFileName.val(this.uploadInput.val().replace(/\\/g, '/').replace(/.*\//, '')));
    this.output.on("load", () => this.outputPanel.hide());

    this.handleResize();

    this.window.keydown((evt) => this.handleKey(evt));
    this.window.resize(() => this.handleResize());

    require(["/sitecore/shell/client/Reference/Assets/TypeAhead/Bootstrap3-TypeAhead.min.js"], () => {
      this.codeHelp.typeahead({ items: 50, source: typeaheaddata });
    });
  }

  handleResize() {
    var height = (this.window.height() - 168);
    this.iframes.height(height);
    this.outputPanel.height(height);
    this.outputPanel.width(this.iframes.width());
  }

  handleKey(evt) {
    if (evt.ctrlKey && evt.keyCode == 13) {
      this.runFiddle();
      evt.preventDefault();
      return false;
    }

    if (evt.ctrlKey && (evt.which == 115 || evt.which == 19 || evt.which == 83)) {
      this.saveFiddle();
      evt.preventDefault();
      return false;
    }

    return true;
  }

  runFiddle() {
    this.outputPanel.show();
    this.action.val("run");
    this.form.submit();
  }

  saveFiddle() {
    this.action.val("save");
    this.form.submit();
  }

  forkFiddle() {
    this.action.val("fork");
    this.form.attr("target", null);
    this.form.submit();
  }

  displayHelp(evt) {
    var options = {
      dataType: "text",
      type: "GET",
      url: "/sitecore/shell/client/Reference/FiddleHelp.ashx?t=" + this.codeHelp.val(),
      success: (response) => this.codeHelpText.html(response)
    };

    $.ajax(options);
  }
}

Sitecore.Speak.pageCode(["bootstrap", "scPipeline"], new FiddlePageCode());
