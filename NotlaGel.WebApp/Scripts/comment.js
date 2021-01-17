var noteid = -1;
var modalCommentBodyId = "#modal_comment_body";

$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");

        $("#modal_comment_body").load("/Comment/ShowNoteComments/" + noteid);
    })
});

function doComment(btn, e, commentid, spanid) {
    var button = $(btn);
    var mode = button.data("edit-mode");


    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnspan = button.find("span");
            btnspan.removeClass("glyphicon-edit");
            btnspan.addClass("glyphicon-ok");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();
        }
        else {
            button.data("edit-mode", false);
            button.removeClass("btn-success");
            button.addClass("btn-warning");
            var btnspan = button.find("span");
            btnspan.removeClass("glyphicon-ok");
            btnspan.addClass("glyphicon-edit");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);


            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {

                if (data.result) {
                    $("#modal_comment_body").load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
                    alert("Yorum güncellenemedi.");
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }
    }
    else if (e === "delete_clicked") {
        var dialog_res = confirm("yorum silinsin mi?");
        if (!dialog_res)
            return false;
        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid
        }).done(function (data) {
            if (data.result) {
                $("#modal_comment_body").load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum silinemedi")
            }
        }).fail(function () {
            alert("sunucu ile bağlantı kurulamadı.")
        });
    }
    else if (e === "new_clicked") {

        var txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { text: txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                $("#modal_comment_body").load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum eklenemedi")
            }
        }).fail(function () {
            alert("sunucu ile bağlantı kurulamadı.")
        });
    }
}