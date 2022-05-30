export function autofocus(parent) {
    const elem = (parent || document).querySelector("[autofocus],.autofocus");
    if (elem) {
        elem.focus();
    }
}