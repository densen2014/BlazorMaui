import Data from "../_content/BootstrapBlazor/modules/data.js";
import EventHandler from "../_content/BootstrapBlazor/modules/event-handler.js";

export function init(id) {
    var el = document.getElementById(id)
    if (el === null) {
        return
    }
    const navbar = el.querySelector('.navbar-toggler')
    const menu = el.querySelector('.sidebar-content')
    const nav = { navbar, menu }
    Data.set(id, nav)

    EventHandler.on(navbar, 'click', () => {
        menu.classList.toggle('show')
    })
    EventHandler.on(menu, 'click', '.nav-link', e => {
        const link = e.delegateTarget
        const url = link.getAttribute('href');
        if (url !== '#') {
            menu.classList.remove('show')
        }
    })
}

export function dispose(id) {
    const nav = Data.get(id)
    if (nav) {
        EventHandler.off(nav.navbar, 'click');
        EventHandler.off(nav.menu, 'click', '.nav-link');
    }
}
