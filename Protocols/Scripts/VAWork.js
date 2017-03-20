function UserData() {
    this.Result = ko.observable("");
    this.Step = ko.observable("0");
    this.IsLoading = ko.observable(false);

    var self = this;
    this.HasResult = ko.computed(function () { return self.Result() !== "" });
    this.IsResultError = ko.computed(function () { return self.Result().startsWith("Ошибка") });
    this.ResultHeader = ko.computed(function () { return self.Result().split(" ", 1) });
    this.ResultText = ko.computed(function () { return self.Result().split(/(?:.*?) (.+)/)[1] });
    this.StepText = ko.computed(function () {return "Этап №" + self.Step()});
}
var data = new UserData();

function loadNewField() {
    $("#Field").empty();
    data.IsLoading(true);

    var sessionId = data.SessionId;
    $.ajax({
        async: true,
        url: "/VisualAuthentication/GenerateNewField",
        cache: false,
        data: { sessionId: sessionId },
        success: function (data) {
            var field = JSON.parse(data);
            updateField(field);
        },
        complete: function () {
            data.IsLoading(false);
        }
    });
}

function updateField(fieldViewModel) {
    var div = $("#Field");

    var field = fieldViewModel.Pics;
    for (var i = 0; i < field.length; ++i) {
        for (var j = 0; j < field[i].length; ++j) {
            var advClick = getAdvClick(i, j, fieldViewModel);
            var imd = '<img src="data:image/bmp;base64, ' + field[i][j] + '" class="img-thumbnail" style="margin: 5px" ' + advClick + ' />';

            div.append(imd);
        }
        div.append("<br/>");
    }

    data.Step(+data.Step() + 1);
}

function getAdvClick(i, j, fieldViewModel) {
    var lastI = i === fieldViewModel.Pics.length - 1;
    var lastJ = j === fieldViewModel.Pics[i].length - 1;

    if ((!lastI && !lastJ) || (lastI && lastJ))
        return "";

    if (lastI)
        return 'onclick="sendAnswer(' + fieldViewModel.ColumnAnswers[j] + ')"';

    return 'onclick="sendAnswer(' + fieldViewModel.RowAnswers[i] + ')"';
}

function sendAnswer(answ) {
    $("#Field").empty();
    data.IsLoading(true);

    var sessionId = data.SessionId;
    var self = data;

    $.ajax({
        async: true,
        url: "/VisualAuthentication/SendAnswer",
        cache: false,
        data: {
            sessionId: sessionId,
            answer: answ
        },
        success: function (data) {
            var jsonRes = JSON.parse(data);
            if (jsonRes.ResultText) {
                self.Result(jsonRes.ResultText);
            } else {
                updateField(jsonRes);    
            }
        },
        complete: function () {
            data.IsLoading(false);
        }
    });
}