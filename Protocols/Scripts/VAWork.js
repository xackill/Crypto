function UserData() {
    this.Result = ko.observable("");
    this.Step = ko.observable("1");

    var self = this;
    this.HasResult = ko.computed(function () { return self.Result() !== "" });
    this.IsResultError = ko.computed(function () { return self.Result().startsWith("Ошибка") });
    this.ResultHeader = ko.computed(function () { return self.Result().split(" ", 1) });
    this.ResultText = ko.computed(function () { return self.Result().split(/(?:.*?) (.+)/)[1] });
    this.StepText = ko.computed(function () {return "Этап №" + self.Step()});
}
var data = new UserData();