define(['knockout'], function (ko) {
    var vm = {
        displayName: 'My Wish List',
        newWish: ko.observable(''),
        //wishes: [{ name: 'Toaster Oven', hover: true }, { name: 'Old Socks', hover: false }, { name: 'A Pony', hover: true }]
        //showExit: ko.observableArray(["blah", "bop", "boo"]),
        //wishes: [{ name: 'Toaster Oven', hover: showExit()[0] }, { name: 'Old Socks', hover: ko.observable("bop") }, { name: 'A Pony', hover: ko.observable("boo") }]
        //showExit: ko.observableArray(["blah", "bop", "cat"])
        wishes: ko.observableArray([]),
        newRow: function(title) {
            this.name = title;
            this.hover = ko.observable(false);
        },
        addWish: function () {
            //this.newWish is by ref, this.newWish() is by val
            var row = new this.newRow(this.newWish());
            this.wishes.push(row);
            $('#addWishModal').modal('hide');
            this.newWish('');
        },
        showDelete: function (row) {
            row.hover(true);
        },
        hideDelete: function (row) {
            row.hover(false);
        }
    };
    return vm;
});