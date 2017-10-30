function UserData() {
    this.UserId = ko.observable("");
    this.Result = ko.observable("");
    this.Mode = ko.observable("Получить конверт");
    this.Amount = ko.observable("");
    this.CheatAmount = ko.observable("");
    this.EnvelopeID = ko.observable("");
    this.ReciverID = ko.observable("");
    this.Process = ko.observable(false);

    var self = this;
    this.HasResult = ko.computed(function () { return self.Result() !== "" });
    this.IsResultError = ko.computed(function () { return self.Result().startsWith("Ошибка") });
    this.ResultHeader = ko.computed(function () { return self.Result().split(" ", 1) });
    this.ResultText = ko.computed(function () { return self.Result().split(/(?:.*?) (.+)/)[1] });

    this.IsWithdrawMode = ko.computed(function () { return self.Mode() === "Получить конверт" });
    this.IsPayMode = ko.computed(function () { return self.Mode() === "Заплатить" });
    this.IsDepositMode = ko.computed(function () { return self.Mode() === "Положить" });
}
var data = new UserData();


function setMode(mode) {
    data.Mode(mode);
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
                url: "/AnonymousCurrency/GetEnvelope",
                cache: false,
                data: { id: id },
                success: function (data) {
                    var envelope = JSON.parse(data);
                    div.append(
                        "<dl class='dl-horizontal'>" +
                        wrapByDl(envelope, "EncryptedContent") +
                        "<br/>" +
                        wrapByDl(envelope, "EncryptedSecrets") +
                        "<br/>" +
                        wrapByDl(envelope, "EncryptedSecretsSigns") +
                        "<br/>" +
                        wrapByDl(envelope, "PublicPrivateKey") +
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

function wrapByDl(envelope, val) {
    var value = envelope[val];
    value = value.match(/.{5}/g).join(" ");
    return "<dt>" + val + "</dt><dd>" + value + "</dd>";
}

function process() {
    data.Process(true);
    data.Result("");

    if (data.IsWithdrawMode()) {
        sendCreateSealedEnvelopeRequest();
    }
    if (data.IsPayMode()) {
        sendPayRequest();
    }
    if (data.IsDepositMode()) {
        sendDepositeRequest();
    }
}

function sendCreateSealedEnvelopeRequest() {
    $.ajax({
        async: true,
        url: "/AnonymousCurrency/CreateSealedEnvelope",
        cache: false,
        data: {
            userId: data.UserId,
            amount: data.Amount() || 0,
            cheatAmount: data.CheatAmount() || 0
        },
        success: function (result) {
            data.Result(result);
            if (!data.IsResultError()) {
                insertNewEnvelope(result);
                updateAccountBalance(-data.Amount());
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

function sendPayRequest() {
    $.ajax({
        async: true,
        url: "/AnonymousCurrency/Pay",
        cache: false,
        data: {
            userId: data.UserId,
            envelopeId: data.EnvelopeID(),
            reciverId: data.ReciverID()
        },
        success: function (result) {
            data.Result(result);
            if (!data.IsResultError() && data.UserId === data.ReciverID())
                insertNewEnvelope(result);
        },
        error: function () {
            data.Result("Ошибка! Возможно, данные введены некорректно.");
        },
        complete: function () {
            data.Process(false);
        }
    });
}

function sendDepositeRequest() {
    $.ajax({
        async: true,
        url: "/AnonymousCurrency/Deposite",
        cache: false,
        data: {
            userId: data.UserId,
            envelopeId: data.EnvelopeID()
        },
        success: function (result) {
            data.Result(result);
            if (!data.IsResultError()) {
                result = result.match(/\d+/);
                updateAccountBalance(result);
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

function insertNewEnvelope(result) {
    var envelopeId = result.match(/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/);
    if (!envelopeId)
        return;
    
    var element = $(
        "<tr onclick=\"showhide('" + envelopeId + "')\">" +
        "<td></td>" +
        "<td>" + envelopeId + "</td>" +
        "<td>" + (data.IsPayMode() ? "Открытый" : "Запечатанный") +"</td>" +
        "</tr>" +
        "<tr id=\"" + envelopeId + "\" style=\"display: none\">" +
        "<td class=\"text-center\" colspan=\"3\">" +
        "<img id=\"load_" + envelopeId + "\" style=\"display: none; width: 27px\" src=\"../../Content/Load.gif\" alt=\"<Load>\" />" +
        "<div id=\"data_" + envelopeId + "\" class=\"text-left\" style=\"display: none\"></div>" +
        "</td>" +
        "</tr>");
    element.insertAfter($("#TableHeader"));
}

function updateAccountBalance(val) {
    var oldVal = data.AccountBalance();
    data.AccountBalance((+oldVal) + (+val));
}