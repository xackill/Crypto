function UserData() {
    this.Nickname = ko.observable("");

    this.CreationInProgress = ko.observable(false);
}
var data = new UserData();

function createAccount() {
    if (data.Nickname() === "")
        return;

    data.CreationInProgress(true);

    var self = this;
    $.ajax({
        async: true,
        url: "CreateAccount",
        cache: false,
        data: { nickname: data.Nickname() },
        success: function(data) {
            window.location.href = "Work/" + data;
        },
        error: function() {
            self.data.CreationInProgress(false);
        }
    });
}
