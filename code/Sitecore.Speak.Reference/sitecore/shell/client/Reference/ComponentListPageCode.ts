import $ = require("jquery");
import Speak = require("sitecore/shell/client/Speak/Assets/lib/core/1.2/SitecoreSpeak");

class ComponentListPageCode extends Speak.PageCode {
  searchinput: JQuery = $("#search");

  initialize() {
    this.searchinput.keydown((evt) => this.cancelEnter(evt));
    this.searchinput.keyup((evt) => this.search(evt));
  }

  cancelEnter(evt) {              
    if (evt.keyCode == 13) {
      evt.preventDefault();
      evt.preventDefault();
      return false;
    }
  }
                   
  search(evt) {
    var text = this.searchinput.val().toUpperCase();
    var rows = $("#components").find("tr");

    rows.each((index: number, element: Element) => {
      var row = $(element);
      var name = row.children().first().text();
      row.toggle(name.toUpperCase().indexOf(text) >= 0);
    });
  }
}

Sitecore.Speak.pageCode(["bootstrap", "scPipeline"], new ComponentListPageCode());
