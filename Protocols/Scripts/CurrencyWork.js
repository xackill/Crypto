function UserData() {
    this.Mode = ko.observable("Перевести");
    this.Error = ko.observable("");
    this.ProcessTransact = ko.observable(false);

    this.SourceId = ko.observable("");
    this.ExtraSourceId = ko.observable("");
    this.DestPublicKey = ko.observable("");
    this.Coins = ko.observable("");

    var self = this;
    this.IsUnion = ko.computed(function () { return self.Mode() === "Объединить" });
    this.IsTransfer = ko.computed(function () { return self.Mode() === "Перевести" });

    this.WalletPublicKey = "";
}
var data = new UserData();

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
                url: "/Currency/GetTransaction",
                cache: false,
                data: { id: id },
                success: function (data) {
                    var transact = JSON.parse(data);
                    div.append(
                        "<dl class='dl-horizontal'>" +
                            wrapByDl(transact, "MinerReward") +
                            "<br/>" +
                            wrapByDl(transact, "SourceId") +
                            wrapByDl(transact, "ExtraSourceId") +
                            "<br/>" +
                            wrapByDl(transact, "ReciverPublicKey") +
                            wrapByDl(transact, "Coins") +
                            "<br/>" +
                            wrapByDl(transact, "SenderPublicKey") +
                            wrapByDl(transact, "SurplusCoins") +
                            wrapByDl(transact, "SenderSign") +
                            "<br/>" +
                            wrapByDl(transact, "VerifierPublicKey") +
                            wrapByDl(transact, "VerifierSign") +
                            "<br/>" +
                            wrapByDl(transact, "MinerPublicKey") +
                            wrapByDl(transact, "ClosingByte") +
                            "<br/>" +
                            wrapByDl(transact, "MinerSign") +
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

function wrapByDl(transact, val) {
    var value = transact[val];
    if (val.endsWith("PublicKey") || val.endsWith("Sign"))
        value = value.match(/.{5}/g).join(" ");
    return "<dt>" + val + "</dt><dd>" + value + "</dd>";
}

function setMode(mode) {
    data.Mode(mode);
}

function processTransact()
{
    data.ProcessTransact(true);
    data.Error("");

    if (data.IsTransfer())
    {
        $.ajax({
            async: true,
            url: "/Currency/CreateTransfer",
            cache: false,
            data: {
                senderPublicKey: data.WalletPublicKey,
                destPublicKey: data.DestPublicKey(),
                sourceId: data.SourceId(),
                coins: data.Coins()
            },
            success: function(result) {
                var json = JSON.parse(result);
                if (!json.Success)
                    data.Error(json.ErrorMsg);
                else
                    insertNewTransaction(json.TransactId);
            },
            error: function() {
                data.Error("Внутренняя ошибка сервера!");
            },
            complete: function() {
                data.ProcessTransact(false);
            }
        });
    }
    else
    {
        $.ajax({
            async: true,
            url: "/Currency/CreateUnion",
            cache: false,
            data: {
                senderPublicKey: data.WalletPublicKey,
                sourceId: data.SourceId(),
                extraSourceId: data.ExtraSourceId()
            },
            success: function (result) {
                var json = JSON.parse(result);
                if (!json.Success)
                    data.Error(json.ErrorMsg);
                else
                    insertNewTransaction(json.TransactId);
            },
            error: function () {
                data.Error("Внутренняя ошибка сервера! Возможно, это связано с ошибками в формате ввода.");
            },
            complete: function () {
                data.ProcessTransact(false);
            }
        });
    }
}

function insertNewTransaction(transId) {
    var element = $(
        "<tr onclick=\"showhide('" + transId + "')\">" +
            "<td></td>" +
            "<td>" + transId + "</td>" +
        "</tr>" +
        "<tr id=\"" + transId + "\" style=\"display: none\">" +
            "<td class=\"text-center\" colspan=\"2\">" +
                "<img id=\"load_" + transId + "\" style=\"display: none; width: 27px\" src=\"../../Content/Load.gif\" alt=\"<Load>\" />" +
                "<div id=\"data_" + transId + "\" class=\"text-left\" style=\"display: none\"></div>" +
            "</td>" +
        "</tr>" );

    element.insertAfter($("#TableHeader"));
}