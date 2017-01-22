function UserData() {
    this.Name = ko.observable("");
    this.Surname = ko.observable("");

    this.CreationInProgress = ko.observable(false);
}
var data = new UserData();

function createWallet() {
    if (data.Name() === "" || data.Surname() === "")
        return;

    data.CreationInProgress(true);

    var self = this;
    $.ajax({
        async: true,
        url: "CreateWallet",
        cache: false,
        data: { name: data.Name(), surname: data.Surname() },
        success: function(data) {
            self.data.CreationInProgress(false);
            window.location.href = "Work/" + data;
        }
    });
}
