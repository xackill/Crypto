function UserData() {
    this.Result = ko.observable("");
    this.Mode = ko.observable("");
    this.Process = ko.observable(false);
    this.LoadingTable = ko.observable(false);

    this.KeyId = ko.observable("");

    var self = this;
    this.HasResult = ko.computed(function () { return self.Result() !== "" });
    this.IsResultError = ko.computed(function () { return self.Result().startsWith("Ошибка") });
    this.ResultHeader = ko.computed(function () { return self.Result().split(" ", 1) });
    this.ResultText = ko.computed(function () {
        var header = self.Result().split(" ", 1);
        var txt = self.Result().substr(header[0].length + 1);
        if (txt) txt = txt.split("\n").join("<br/>");
        return txt;
    });

    this.IsClientMode = ko.computed(function () { return self.Mode() === "Клиент" });
    this.IsDepositCenterMode = ko.computed(function () { return self.Mode() === "Центр депонирования" });
    this.IsTrustedCenterMode = ko.computed(function () { return self.Mode() === "Доверенные лица" });
    this.IsStateMode = ko.computed(function () { return self.Mode() === "Госорганы" });
}
var data = new UserData();


function setMode(mode) {
    data.Mode(mode);
    data.Result("");
    loadKeyViewer();
}

function loadKeyViewer() {
    $("#KeyViewer").empty();
    data.LoadingTable(true);

    var type = data.IsClientMode() ? "0" :
                data.IsDepositCenterMode() ? "2" :
                    data.IsTrustedCenterMode() ? "3" :  "1";

    $.ajax({
        async: true,
        url: "/KeyDeposit/LoadKeys",
        cache: false,
        data: { keeper: type },
        success: function (answ) {
            if (answ.startsWith("Ошибка"))
                data.Result(answ);
            else {
                var keyIds = JSON.parse(answ);
                createHeaderToKeyViewer();
                loadIdsToKeyViewer(keyIds);
            }
        },
        complete: function () {
            data.LoadingTable(false);
        }
    });
}

function createHeaderToKeyViewer() {
    $("#KeyViewer").append(
        '<tr id="TableHeader">' +
            '<th></th>' +
            '<th>Идентификатор записи</th>' +
        '</tr>'
    );
}

function loadIdsToKeyViewer(keyIds) {
    for (var i = 0; i < keyIds.length; ++i) {
        insertNewKeyToKeyViewer(keyIds[i]);
    }
}

function insertNewKeyToKeyViewer(keyId) {
    $("#KeyViewer").append(
        '<tr onclick="showhide(\'' + keyId + '\')">' +
            '<td></td>' +
            '<td>' + keyId + '</td>' +
        '</tr>' +
        '<tr id="' + keyId + '" style="display: none">' +
            '<td class="text-center" colspan="2">' +
                '<img id="load_' + keyId + '" style="display: none; width: 27px" src="../../Content/Load.gif" alt="<Load>" />' +
                '<div id="data_' + keyId + '" class="text-left" style="display: none"></div>' +
            '</td>' +
        '</tr>'
    );
}

function showhide(id) {
    var tr = $("#" + id);
    if (tr.is(":visible"))
        tr.hide();
    else {
        tr.show();

        var load = $("#load_" + id);
        var div = $("#data_" + id);
        if (!load.is(":visible") && !div.is(":visible")) {
            load.show();

            $.ajax({
                async: true,
                url: "/KeyDeposit/GetKeyContainer",
                cache: false,
                data: { id: id },
                success: function (data) {
                    var container = JSON.parse(data);
                    div.append(
                        "<dl class='dl-horizontal'>" +
                        wrapByDl(container, "KeyId") +
                        "<br/>" +
                        wrapByDl(container, "PublicKey") +
                        wrapByDl(container, "PrivateKey") +
                        wrapByDl(container, "Modulus") +
                        "</dl>"
                    );
                },
                complete: function () {
                    load.hide();
                    div.show();
                }
            });
        }
    }
}

function wrapByDl(container, val) {
    var value = container[val];
    if (!val.endsWith("Id"))
        value = value.match(/.{5}/g).join(" ");
    return "<dt>" + val + "</dt><dd>" + value + "</dd>";
}

function createKey() {
    data.Process(true);
    data.Result("");

    $.ajax({
        async: true,
        url: "/KeyDeposit/CreateKey",
        cache: false,
        success: function (result) {
            data.Result(result);
            if (!data.IsResultError()) {
                insertNewKey(result);
            }
        },
        error: function () {
            data.Result("Ошибка! Возможно, данные введены некорректно.");
        },
        complete: function () {
            data.Process(false);
        }
    });
}

function insertNewKey(result) {
    var keyId = result.match(/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/);
    if (!keyId)
        data.Result("Ошибка! В сообщении не обнаружен ID: " + result);

    insertNewKeyToKeyViewer(keyId);
}

function learnTheKey() {
    data.Process(true);
    data.Result("");

    var keyId = data.KeyId();

    $.ajax({
        async: true,
        url: "/KeyDeposit/LearnTheKey",
        data: { id: keyId },
        cache: false,
        success: function (result) {
            data.Result(result);
            if (!data.IsResultError()) {
                insertNewKey(result);
            }
        },
        error: function () {
            data.Result("Ошибка! Возможно, данные введены некорректно.");
        },
        complete: function () {
            data.Process(false);
        }
    });
}