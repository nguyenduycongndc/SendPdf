/////// <reference path="customer.js" />
//<reference path="host.js" />
var apiConfig = {
    "api": {
        "host_user_service": hostApi.host_user_service,
        "login": {
            "controller": "",
            "action": {
                "sendotp": {
                    "method": "POST",
                    "path": "/SendOTP"
                },
                "forgotpassword": {
                    "method": "POST",
                    "path": "/ForgotPassWord"
                }
            }
        },
        "user": {
            "controller": "/api/User",
            "action": {
                "getItem": {
                    "method": "GET",
                    "path": "/Detail"
                },
                "search": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/Search"
                },
                "add": {
                    //function support to add item
                    "method": "POST",
                    "path": "/Create"
                },
                "delete": {
                    //function support to delete item
                    "method": "DELETE",
                    "path": "/Delete"
                },
                "update": {
                    //function support to update item
                    "method": "PUT",
                    "path": "/Update"
                },
                "changepassword": {
                    //function support to update item
                    "method": "PUT",
                    "path": "/ChangePassWord"
                },
                "editinformation": {
                    //function support to update item
                    "method": "PUT",
                    "path": "/EditInformation"
                }
            }
        },
        "account": {
            "controller": "/api/Account",
            "action": {
                "searchaccount": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccount"
                }, "searchaccounthero": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountHero"
                }, "searchaccountitem": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountItem"
                }, "searchaccountticket": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountTicket"
                }, "searchaccountpack": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountPack"
                }, "searchaccountegg": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountEgg"
                }, "searchaccountshard": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountShard"
                }, "searchaccountref": {
                    //function support get items by search condition
                    "method": "POST",
                    "path": "/SearchAccountRef"
                },
                "getItem": {
                    "method": "GET",
                    "path": "/Detail"
                }
            }
        },
        "sendmail": {
            "controller": "/api/SendMail",
            "action": {
                "searchemail": {
                    "method": "POST",
                    "path": "/SearchEmail"
                },
                "getItem": {
                    "method": "GET",
                    "path": "/DetailEmail"
                },
                "addEmail": {
                    //function support to add item
                    "method": "POST",
                    "path": "/CreateEmail"
                },
                "deleteEmail": {
                    //function support to delete item
                    "method": "DELETE",
                    "path": "/DeleteEmail"
                },
                "updateEmail": {
                    //function support to update item
                    "method": "PUT",
                    "path": "/UpdateEmail"
                },
                "exportexcel": {
                    "method": "GET",
                    "path": "/ExportExcel"
                },
                "sendemail": {
                    "method": "POST",
                    "path": "/SendEmail"
                },
                "importexcel": {
                    "method": "POST",
                    "path": "/ImportExcel"
                }
            }
        },
        "dataemail": {
            "controller": "/api/DataEmail",
            "action": {
                //"searchemail": {
                //    "method": "POST",
                //    "path": "/SearchEmail"
                //},
                //"getItem": {
                //    "method": "GET",
                //    "path": "/DetailEmail"
                //},
                "savedataemail": {
                    "method": "POST",
                    "path": "/SaveDataEmail"
                },
                "dataemaildetail": {
                    "method": "GET",
                    "path": "/DataEmailDetail"
                },
            }
        },
        "sendfilepdf": {
            "controller": "/api/SendMailPDF",
            "action": {
                "sendpdf": {
                    "method": "POST",
                    "path": "/SendFilePDF"
                },
                "importexcelpdf": {
                    "method": "POST",
                    "path": "/ImportExcelPDF"
                }
            }
        },
    },
};

