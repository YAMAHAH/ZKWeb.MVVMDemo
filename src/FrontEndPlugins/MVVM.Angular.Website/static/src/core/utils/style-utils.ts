
export class styleUntils {
    static setupStyleEl(el: Element, style: string) {
        let styleEl: HTMLStyleElement = document.createElement('style');
        let styleHTML = style;
        styleEl.innerHTML = styleHTML;

        if (el) {
            el.appendChild(styleEl);
        }
        return () => el.removeChild(styleEl);
    }

    static allowUserSelect(allowSelect: boolean = false) {
        let styleHtml = ` 
        body {
          -webkit-user-select:none;
          user-select:none;
        }`;
        if (allowSelect) {
            styleHtml = ` 
        body {
          -webkit-user-select:text;
          user-select:text;
        }`;
        }
        return styleUntils.setupStyleEl(document.body, styleHtml);
    }
}