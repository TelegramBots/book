// Populate the sidebar
//
// This is a script, and not included directly in the page, to control the total size of the book.
// The TOC contains an entry for each page, so if each page includes a copy of the TOC,
// the total size of the page becomes O(n**2).
class MDBookSidebarScrollbox extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        this.innerHTML = '<ol class="chapter"><li class="chapter-item expanded "><a href="index.html"><strong aria-hidden="true">1.</strong> Introduction</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="1/quickstart.html"><strong aria-hidden="true">1.1.</strong> Quickstart</a></li><li class="chapter-item "><a href="1/example-bot.html"><strong aria-hidden="true">1.2.</strong> First Chat Bot</a></li><li class="chapter-item "><a href="1/full-bot.html"><strong aria-hidden="true">1.3.</strong> Full Example</a></li></ol></li><li class="chapter-item expanded "><a href="2/index.html"><strong aria-hidden="true">2.</strong> Beginner</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="2/send-msg/index.html"><strong aria-hidden="true">2.1.</strong> Sending Messages</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="2/send-msg/media-msg.html"><strong aria-hidden="true">2.1.1.</strong> Sending Media</a></li><li class="chapter-item "><a href="2/send-msg/native-polls-msg.html"><strong aria-hidden="true">2.1.2.</strong> Native Polls</a></li><li class="chapter-item "><a href="2/send-msg/other-msg.html"><strong aria-hidden="true">2.1.3.</strong> Other Messages</a></li></ol></li><li class="chapter-item "><a href="2/chats.html"><strong aria-hidden="true">2.2.</strong> Dealing with chats</a></li><li class="chapter-item "><a href="2/reply-markup.html"><strong aria-hidden="true">2.3.</strong> Reply Markup</a></li><li class="chapter-item "><a href="2/forward-copy-delete.html"><strong aria-hidden="true">2.4.</strong> Forward, Copy or Delete</a></li></ol></li><li class="chapter-item expanded "><a href="3/index.html"><strong aria-hidden="true">3.</strong> Intermediate</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="3/updates/index.html"><strong aria-hidden="true">3.1.</strong> Working with Updates</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="3/updates/polling.html"><strong aria-hidden="true">3.1.1.</strong> Long Polling</a></li><li class="chapter-item "><a href="3/updates/webhook.html"><strong aria-hidden="true">3.1.2.</strong> Webhooks</a></li></ol></li><li class="chapter-item "><a href="3/files.html"><strong aria-hidden="true">3.2.</strong> Download/Upload Files</a></li><li class="chapter-item "><a href="3/inline.html"><strong aria-hidden="true">3.3.</strong> Inline Mode</a></li><li class="chapter-item "><a href="3/helpers.html"><strong aria-hidden="true">3.4.</strong> Library helpers</a></li></ol></li><li class="chapter-item expanded "><a href="4/index.html"><strong aria-hidden="true">4.</strong> Advanced</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="4/proxy.html"><strong aria-hidden="true">4.1.</strong> Proxy</a></li><li class="chapter-item "><a href="4/business.html"><strong aria-hidden="true">4.2.</strong> Business Features</a></li><li class="chapter-item "><a href="4/payments.html"><strong aria-hidden="true">4.3.</strong> Payments API</a></li><li class="chapter-item "><a href="4/webapps.html"><strong aria-hidden="true">4.4.</strong> Mini Apps</a></li><li class="chapter-item "><a href="4/passport/index.html"><strong aria-hidden="true">4.5.</strong> Passport</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="4/passport/files-docs.html"><strong aria-hidden="true">4.5.1.</strong> Files &amp; Documents</a></li><li class="chapter-item "><a href="4/passport/errors.html"><strong aria-hidden="true">4.5.2.</strong> Data Errors</a></li></ol></li></ol></li><li class="chapter-item expanded "><a href="FAQ.html"><strong aria-hidden="true">5.</strong> Frequently Asked Questions</a></li><li class="chapter-item expanded "><a href="migrate/index.html"><strong aria-hidden="true">6.</strong> Migration guides to newer versions</a><a class="toggle"><div>❱</div></a></li><li><ol class="section"><li class="chapter-item "><a href="migrate/Version-22.x.html"><strong aria-hidden="true">6.1.</strong> Migrate to v22.*</a></li><li class="chapter-item "><a href="migrate/Version-21.x.html"><strong aria-hidden="true">6.2.</strong> Migrate to v21.*</a></li><li class="chapter-item "><a href="migrate/Version-19.x.html"><strong aria-hidden="true">6.3.</strong> Migrate to v19.*</a></li><li class="chapter-item "><a href="migrate/Version-18.x.html"><strong aria-hidden="true">6.4.</strong> Migrate to v18.*</a></li><li class="chapter-item "><a href="migrate/Version-17.x.html"><strong aria-hidden="true">6.5.</strong> Migrate to v17.*</a></li><li class="chapter-item "><a href="migrate/Version-14.x.html"><strong aria-hidden="true">6.6.</strong> Migrate to v14.*</a></li></ol></li></ol>';
        // Set the current, active page, and reveal it if it's hidden
        let current_page = document.location.href.toString().split("#")[0].split("?")[0];
        if (current_page.endsWith("/")) {
            current_page += "index.html";
        }
        var links = Array.prototype.slice.call(this.querySelectorAll("a"));
        var l = links.length;
        for (var i = 0; i < l; ++i) {
            var link = links[i];
            var href = link.getAttribute("href");
            if (href && !href.startsWith("#") && !/^(?:[a-z+]+:)?\/\//.test(href)) {
                link.href = path_to_root + href;
            }
            // The "index" page is supposed to alias the first chapter in the book.
            if (link.href === current_page || (i === 0 && path_to_root === "" && current_page.endsWith("/index.html"))) {
                link.classList.add("active");
                var parent = link.parentElement;
                if (parent && parent.classList.contains("chapter-item")) {
                    parent.classList.add("expanded");
                }
                while (parent) {
                    if (parent.tagName === "LI" && parent.previousElementSibling) {
                        if (parent.previousElementSibling.classList.contains("chapter-item")) {
                            parent.previousElementSibling.classList.add("expanded");
                        }
                    }
                    parent = parent.parentElement;
                }
            }
        }
        // Track and set sidebar scroll position
        this.addEventListener('click', function(e) {
            if (e.target.tagName === 'A') {
                sessionStorage.setItem('sidebar-scroll', this.scrollTop);
            }
        }, { passive: true });
        var sidebarScrollTop = sessionStorage.getItem('sidebar-scroll');
        sessionStorage.removeItem('sidebar-scroll');
        if (sidebarScrollTop) {
            // preserve sidebar scroll position when navigating via links within sidebar
            this.scrollTop = sidebarScrollTop;
        } else {
            // scroll sidebar to current active section when navigating via "next/previous chapter" buttons
            var activeSection = document.querySelector('#sidebar .active');
            if (activeSection) {
                activeSection.scrollIntoView({ block: 'center' });
            }
        }
        // Toggle buttons
        var sidebarAnchorToggles = document.querySelectorAll('#sidebar a.toggle');
        function toggleSection(ev) {
            ev.currentTarget.parentElement.classList.toggle('expanded');
        }
        Array.from(sidebarAnchorToggles).forEach(function (el) {
            el.addEventListener('click', toggleSection);
        });
    }
}
window.customElements.define("mdbook-sidebar-scrollbox", MDBookSidebarScrollbox);
