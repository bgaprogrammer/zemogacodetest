//ViewModel

var transactionsViewModel = kendo.observable({
    user: "",
    pwd: "",
    canCreateTransaction: false,
    canMarkTransactionAsFraud: false,
    onShowLoginView: function () {
        //Attaches a kendoValidator to the login form
        $("#loginForm").kendoValidator().data("kendoValidator");
    },
    onBtnloginClick: function (e) {
        e.preventDefault();

        var loginValidator = $("#loginForm").kendoValidator().data("kendoValidator");

        if (loginValidator.validate()) {
            transactionsViewModel.requestNewAuthToken();
        }
    },
    requestNewAuthToken: function () {
        kendo.ui.progress($("#loginForm"), true);

        $.ajax({
            type: "POST",
            url: "/api/account/login",
            contentType: "application/json",
            accepts: "application/json",
            cache: false,
            data: JSON.stringify({
                Email: transactionsViewModel.get("user"),
                Password: transactionsViewModel.get("pwd")
            }),
            success: function (result, status, xhr) {
                kendo.ui.progress($("#loginForm"), false);

                transactionsViewModel.setAuthToken(result.token);
                transactionsViewModel.setAuthRoles(result.roles);
                transactionsViewModel.redirectToTransactionsView();
            },
            error: function (xhr, status, error) {
                kendo.ui.progress($("#loginForm"), false);

                if (xhr.status == 400) {
                    kendo.alert("Wrong data. Try again");
                    return;
                }

                kendo.alert("[" + xhr.status + "] " + status);
            }
        });
    },
    setAuthToken: function (token) {
        sessionStorage.setItem("AuthToken", token);
    },
    setAuthRoles: function(roles) {
        sessionStorage.setItem("AuthRoles", roles);
    },
    getAuthToken: function () {
        var token = sessionStorage.getItem("AuthToken");

        if (!token || token.toString().length == 0) {
            transactionsViewModel.redirectToLoginView();
            return;

            //If an invalid token is setted in the storage, those cases will be handled by each ajax request;
        }

        return "Bearer " + token.toString();
    },
    isUserInRole: function(role) {
        var roles = sessionStorage.getItem("AuthRoles");

        if (!roles || roles.toString().length == 0) {
            transactionsViewModel.redirectToLoginView();
            return;

            //Roles are only part of the navigation process, enable or disable options
        }

        return (roles.toString().toLowerCase().search(role.toLowerCase()) >= 0);
    },
    redirectToLoginView: function () {
        router.navigate("/");
    },
    redirectToTransactionsView: function () {
        router.navigate("/transactions");
    },
    onShowTransactionsView: function () {
        //Attaches a kendoValidator to the transactions form
        $("#transactionsForm").kendoValidator().data("kendoValidator");

        transactionsViewModel.set("canCreateTransaction", (transactionsViewModel.isUserInRole("Assistant") || transactionsViewModel.isUserInRole("Administrator")));
        transactionsViewModel.set("canMarkTransactionAsFraud", (transactionsViewModel.isUserInRole("Manager") || transactionsViewModel.isUserInRole("Administrator")));

        if (transactionsViewModel.get("canCreateTransaction")) {
            $(".k-grid-add").removeClass("k-state-disabled").addClass("k-grid-add");
        } else {
            $(".k-grid-add").addClass("k-state-disabled").removeClass("k-grid-add");
        }
    },
    transactionsDS: new kendo.data.DataSource({
        pageSize: 10,
        serverFiltering: true,
        serverPaging: true,
        serverSorting: true,
        transport: {
            read: {
                url: "/api/transaction/list",
                contentType: "application/json",
                accepts: "application/json",
                type: "POST",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", transactionsViewModel.getAuthToken());
                }
            },
            create: {
                url: "/api/transaction",
                contentType: "application/json",
                accepts: "application/json",
                type: "POST",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", transactionsViewModel.getAuthToken());
                }
            },
            update: {
                url: "/api/transaction",
                contentType: "application/json",
                accepts: "application/json",
                type: "PUT",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", transactionsViewModel.getAuthToken());
                }
            },
            parameterMap: function (options, operation) {
                if (options) {
                    return kendo.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            total: "total",
            model: {
                id: "id",
                fields: {
                    id: {
                        type: "numeric"
                    },
                    step: {
                        type: "numeric"
                    },
                    amount: {
                        type: "numeric"
                    },
                    originCustomerId: {
                        type: "numeric"
                    },
                    destCustomerId: {
                        type: "numeric"
                    },
                    TransactionTypeId: {
                        type: "numeric"
                    },
                    executionDate: {
                        type: "date"
                    },
                    destCustomer: {
                        defaultValue: {}
                    },
                    originCustomer: {
                        defaultValue: {}
                    },
                    transactionType: {
                        defaultValue: {}
                    },
                    transactionDetail: {
                        defaultValue: {}
                    }
                }
            }
        },
        requestStart: function (e) {
            kendo.ui.progress($("#transactionsGrid"), true);
        },
        requestEnd: function (e) {
            kendo.ui.progress($("#transactionsGrid"), false);
        },
        error: function (e) {
            console.log("transactionsDS Error: " + kendo.stringify(e));
        }
    }),
    onDataBoundTransactionsGrid: function (e) {
        $(".k-command").each(function (index) {
            if (transactionsViewModel.get("canMarkTransactionAsFraud")) {
                $(this).removeClass("k-state-disabled").addClass("k-grid-edit");
            } else {
                $(this).addClass("k-state-disabled").removeClass("k-grid-edit");
            }
        });
    },
    onEditTransactionsGrid: function (e) {
        if (e.model.isNew()) {
            $(".k-window-title").text("New Transaction");
            $(".k-grid-update").text("Add");

            $("#ddlTranTypeModal").data("kendoDropDownList").enable(true);
            $("#ddlCustOriginModal").data("kendoDropDownList").enable(true);
            $("#ddlCustDestModal").data("kendoDropDownList").enable(true);
            $("#dpTransactionDateModal").data("kendoDatePicker").enable(true);
            $("#ntxtTransactionAmountModal").data("kendoNumericTextBox").enable(true);
            $("#chkIsFraudModal").prop("disabled", true);
        } else {
            $(".k-window-title").text("Update Transaction");
            $(".k-grid-update").text("Update");

            $("#ddlTranTypeModal").data("kendoDropDownList").enable(false);
            $("#ddlCustOriginModal").data("kendoDropDownList").enable(false);
            $("#ddlCustDestModal").data("kendoDropDownList").enable(false);
            $("#dpTransactionDateModal").data("kendoDatePicker").enable(false);
            $("#ntxtTransactionAmountModal").data("kendoNumericTextBox").enable(false);
            $("#chkIsFraudModal").prop("disabled", false);
        }

        //Sets Transaction Date min date
        var dpTransactionDateModal = $("#dpTransactionDateModal").data("kendoDatePicker");
        dpTransactionDateModal.min(new Date());

        //Attach select event to the Origin Customer ddl
        var ddlCustOriginModal = $("#ddlCustOriginModal").data("kendoDropDownList");
        ddlCustOriginModal.bind("select", transactionsViewModel.get("onSelectddlCustOriginModal"));

        //Attach select event to the Destination Customer ddl
        var ddlCustDestModal = $("#ddlCustDestModal").data("kendoDropDownList");
        ddlCustDestModal.bind("select", transactionsViewModel.get("onSelectddlCustDestModal"));
    },
    tranTypeDS: new kendo.data.DataSource({
        pageSize: 10,
        serverFiltering: true,
        serverPaging: true,
        transport: {
            read: {
                url: "/api/transaction/type",
                contentType: "application/json",
                accepts: "application/json",
                type: "POST",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", transactionsViewModel.getAuthToken());
                }
            },
            parameterMap: function (options, operation) {
                if (options) {
                    return kendo.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            total: "total",
            model: {
                id: "id",
                fields: {
                    id: {
                        type: "numeric"
                    },
                    name: {
                        type: "string"
                    }
                }
            }
        }
    }),
    customerOriginDS: new kendo.data.DataSource({
        pageSize: 10,
        serverFiltering: true,
        serverPaging: true,
        transport: {
            read: {
                url: "/api/transaction/customer",
                contentType: "application/json",
                accepts: "application/json",
                type: "POST",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", transactionsViewModel.getAuthToken());
                }
            },
            parameterMap: function (options, operation) {
                if (options) {
                    return kendo.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            total: "total",
            model: {
                id: "id",
                fields: {
                    id: {
                        type: "numeric"
                    },
                    name: {
                        type: "string"
                    }
                }
            }
        }
    }),
    customerDestinationDS: new kendo.data.DataSource({
        pageSize: 10,
        serverFiltering: true,
        serverPaging: true,
        transport: {
            read: {
                url: "/api/transaction/customer",
                contentType: "application/json",
                accepts: "application/json",
                type: "POST",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", transactionsViewModel.getAuthToken());
                }
            },
            parameterMap: function (options, operation) {
                if (options) {
                    return kendo.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            total: "total",
            model: {
                id: "id",
                fields: {
                    id: {
                        type: "numeric"
                    },
                    name: {
                        type: "string"
                    }
                }
            }
        }
    }),
    onSelectddlCustOriginModal: function (e) {
        var ddlCustDestModal = $("#ddlCustDestModal").data("kendoDropDownList");

        if (e.sender.value().length == 0 && ddlCustDestModal.value().length > 0) {
            ddlCustDestModal.value("");
        }

        var selectedCustOrigin = e.sender.dataItem(e.item);

        if (selectedCustOrigin) {

            var ntxtTransactionAmountModal = $("#ntxtTransactionAmountModal").data("kendoNumericTextBox");

            //Validates the current balance of the origin customer
            if (selectedCustOrigin.lastBalance == 0) {
                kendo.alert("The Balance for the selected origin customer is 0. Select another");
                ntxtTransactionAmountModal.value(0);
                ntxtTransactionAmountModal.trigger("change");

                e.preventDefault(); //Prevents the selection of the item
                return;
            }

            //If balance is greater than 0 it's set as the maximum valid value of the amount numerictextbox
            ntxtTransactionAmountModal.max(selectedCustOrigin.lastBalance);

            var currentValue = ntxtTransactionAmountModal.value();

            if (currentValue) {

                //If there is already a value in the numerictextbox it's needed to validate if is greater than the maximum balance of the customer
                if (currentValue > selectedCustOrigin.lastBalance) {
                    ntxtTransactionAmountModal.value(selectedCustOrigin.lastBalance);
                    ntxtTransactionAmountModal.trigger("change");
                }
            }
        }
    },
    onSelectddlCustDestModal: function (e) {
        var ddlCustOriginModal = $("#ddlCustOriginModal").data("kendoDropDownList");

        if (ddlCustOriginModal.value().length == 0) {
            kendo.alert("You must select an Origin Customer first");
            e.preventDefault(); //Prevents the selection of the item
            return;
        }

        var selectedCustDest = e.sender.dataItem(e.item);

        if (selectedCustDest) {

            //The origin and destination customers can't be the same
            if (selectedCustDest.id == ddlCustOriginModal.dataItem().id) {
                kendo.alert("The Destination customer can't be the same as Origin");

                e.preventDefault(); //Prevents the selection of the item
                return;
            }
        }
    }
});

//Layout and Views

var layout = new kendo.Layout("<div class='container-fluid'><section id='content'></section></div>");

var loginView = new kendo.View($("#login-view").html(), {
    model: transactionsViewModel,
    show: transactionsViewModel.onShowLoginView.bind(transactionsViewModel)
});

var transactionsView = new kendo.View($("#transactions-view").html(), {
    model: transactionsViewModel,
    init: function () {
        transactionsViewModel.getAuthToken();
    },
    show: transactionsViewModel.onShowTransactionsView.bind(transactionsViewModel)
});

//Routing

var router = new kendo.Router();

router.bind("init", function () {
    layout.render($("#app"));
});

router.route("/", function () {
    layout.showIn("#content", loginView);
});

router.route("/transactions", function () {
    layout.showIn("#content", transactionsView);
});

$(function () {
    router.start();
});