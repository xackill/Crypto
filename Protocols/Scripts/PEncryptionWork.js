function UserData() {
    this.Result = ko.observable("");
    this.Mode = ko.observable("");
    this.Process = ko.observable(false);
    this.LoadingTable = ko.observable(false);

    this.KeyId = ko.observable("");
    this.MessageId = ko.observable("");
    this.Message = ko.observable("");

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

    this.IsKeyMode = ko.computed(function () { return self.Mode() === "Создать ключ" });
    this.IsEncryptMode = ko.computed(function () { return self.Mode() === "Зашифровать" });
    this.IsDecryptMode = ko.computed(function () { return self.Mode() === "Расшифровать" });

    this.TableHeader = ko.computed(function () { return self.IsKeyMode() ? "Доступные контейнеры" : "Зашифрованные сообщения"; });
    this.LoadIdServerMethod = ko.computed(function () { return self.IsKeyMode() ? "LoadKeyContainersIds" : "LoadEncryptedMessagesIds"; });
    this.LoadContainerServerMethod = ko.computed(function () { return self.IsKeyMode() ? "LoadKeyContainer" : "LoadEncryptedMessage"; });
    this.CreateServerMethod = ko.computed(function() {
        return self.IsKeyMode() ? "CreateKeyContainer" : self.IsEncryptMode() ? "CreateEncryptedMessage" : "DecryptMessage";
    });
}
var data = new UserData();


function setMode(mode) {
    data.Mode(mode);
    data.Result("");

    if (!data.IsDecryptMode())
        loadViewer();
}

function loadViewer() {
    $("#Viewer").empty();
    data.LoadingTable(true);
    
    $.ajax({
        async: true,
        url: "/ProbabilisticEncryption/" + data.LoadIdServerMethod(),
        cache: false,
        success: function (answ) {
            if (answ.startsWith("Ошибка"))
                data.Result(answ);
            else {
                var keyIds = JSON.parse(answ);
                createHeaderToViewer();
                loadIdsToViewer(keyIds);
            }
        },
        complete: function () {
            data.LoadingTable(false);
        }
    });
}

function createHeaderToViewer() {
    $("#Viewer").append(
        '<tr id="TableHeader">' +
            '<th></th>' +
            '<th>Идентификатор записи</th>' +
        '</tr>'
    );
}

function loadIdsToViewer(keyIds) {
    for (var i = 0; i < keyIds.length; ++i) {
        insertNewKeyToViewer(keyIds[i]);
    }
}

function insertNewKeyToViewer(keyId) {
    $("#Viewer").append(
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
                url: "/ProbabilisticEncryption/" + data.LoadContainerServerMethod(),
                cache: false,
                data: { id: id },
                success: function (data) {
                    var container = JSON.parse(data);
                    addContainer(div, container);
                },
                complete: function () {
                    load.hide();
                    div.show();
                }
            });
        }
    }
}

function addContainer(div, container) {
    if (data.IsKeyMode())
        div.append(
            "<dl class='dl-horizontal'>" +
            wrapByDl(container, "P") +
            wrapByDl(container, "Q") +
            "</dl>"
        );
    else
        div.append(
            "<dl class='dl-horizontal'>" +
            wrapByDl(container, "Message") +
            wrapByDl(container, "Xt") +
            "</dl>"
        );
}

function wrapByDl(container, val) {
    var value = container[val];
    value = value.match(/.{5}/g).join(" ");
    return "<dt>" + val + "</dt><dd>" + value + "</dd>";
}

function process() {
    data.Process(true);
    data.Result("");

    var sData = {};
    if (data.IsEncryptMode()) {
        sData.keyId = data.KeyId();
        sData.message = data.Message();
    }
    if (data.IsDecryptMode()) {
        sData.keyId = data.KeyId();
        sData.messageId = data.MessageId();
    }

    $.ajax({
        async: true,
        url: "/ProbabilisticEncryption/" + data.CreateServerMethod(),
        cache: false,
        data: sData,
        success: function (result) {
            data.Result(result);
            if (!data.IsResultError() && !data.IsDecryptMode()) {
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

    insertNewKeyToViewer(keyId);
}