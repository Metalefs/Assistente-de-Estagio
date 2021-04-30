document.addEventListener('DOMContentLoaded', function () {
    $(document).ready(function () {

        const SideBarCookie = "Sidenav-open";
        const MSideBarCookie = "MSidenav-open";
        const AdmTabCookie = "AdmTab-open";
        const UserTabCookie = "UserTab-open";

        $("#sidebarCollapse").click(() => {
            ToggleSideNav();
            if (window.innerWidth >= 780) {
                $(".sidenav-overlay").click();
            }
        });

        function ToggleSideNav(state) {
            if (state === "open") {
                $('#sidebar').addClass('active');
                $('#sidebar-column').addClass('active');
                $('.MainContainer').addClass('active');
            }
            else if (state === "close") {
                $('#sidebar').removeClass('active');
                $('#sidebar-column').removeClass('active');
                $('.MainContainer').removeClass('active');
            }
            else {
                $('#sidebar').toggleClass('active');
                $('#sidebar-column').toggleClass('active');
                $('.MainContainer').toggleClass('active');
            }

            sessionStorage.setItem(MSideBarCookie, $('#slide-out')[0].className);
            sessionStorage.setItem(SideBarCookie, !$('#sidebar').hasClass('active'));
        }

        $("#adm-tab-select").click(() => {
            setTimeout(() => {
                sessionStorage.setItem(AdmTabCookie, $('#admin-tab-body').attr("style"));
            }, 1000);
        });
        $("#user-tab-select").click(() => {
            setTimeout(() => {
                sessionStorage.setItem(UserTabCookie, $('#user-tab-body').attr("style"));
            }, 1000);
        });

        (function HandleSideNavState() {
            let mclass = sessionStorage.getItem(MSideBarCookie);
            if (mclass !== null) {
                if ($("#slide-out").length > 0)
                $("#slide-out")[0].className = mclass;
            }

            if (sessionStorage.getItem(SideBarCookie) === "true" || window.innerWidth < 780) {
                ToggleSideNav("close");
                $("sidebar-cursos").show();
            }
            else if (sessionStorage.getItem(SideBarCookie) === null) {
                if(window.location.pathname !== "/")
                    ToggleSideNav("open");
            }
        })();

        (function HandleAdmTabState() {
            let admclass = sessionStorage.getItem(AdmTabCookie);
            let usrclass = sessionStorage.getItem(UserTabCookie);
            if (admclass !== null) {
                $('#admin-tab-body').attr("style",admclass);
            }
            if (usrclass !== null) {
                $('#user-tab-body').attr("style",usrclass);
            }
        })();
    });
});