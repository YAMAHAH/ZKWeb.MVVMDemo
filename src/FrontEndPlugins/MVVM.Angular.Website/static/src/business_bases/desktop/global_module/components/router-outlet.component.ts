import { Component } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'router-outlet-container',
    template: `
        <router-outlet></router-outlet>
        <router-outlet name ="sale-o"></router-outlet>
        <router-outlet name ="sale-o-q"></router-outlet>
    `
})
export class RouterOutletComponent {

}
