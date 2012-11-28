(function () {
    window.UserDataImporter = function (options) {
        var self = this;
        var settings = {
            btnLoad: null,
            targetContainer: null,
            txtGoogleId: null,
            loadingIndicator: null,
            userId: null
        };

        $.extend(self, settings, options);

        self.btnLoad = $(self.btnLoad);
        self.targetContainer = $(self.targetContainer);
        self.loadingIndicator = $(self.loadingIndicator);

        $(self.txtGoogleId).live('keyup change', function () {
            self._toggleButton();
        });

        self.btnLoad.click(function () {
            self.LoadUserData();
        });

        self._toggleButton();
    };

    window.UserDataImporter.prototype = {
        _toggleButton: function () {
            var self = this;

            if ($(self.txtGoogleId).val() != '') {
                self.btnLoad.show();
            } else {
                self.btnLoad.hide();
            }
        },
        LoadUserData: function () {
            var self = this;
            $('input', self.targetContainer).attr('disabled', 'disabled');
            self.btnLoad.attr('disabled', 'disabled');
            self.loadingIndicator.show();

            var params = $.param({
                googleId: $(self.txtGoogleId).val(),
                userId: self.userId
            });

            self.targetContainer.load('GetUserForm?' + params, function () {
                self.loadingIndicator.hide();
                self.btnLoad.removeAttr('disabled');
            });
        }
    };
})();