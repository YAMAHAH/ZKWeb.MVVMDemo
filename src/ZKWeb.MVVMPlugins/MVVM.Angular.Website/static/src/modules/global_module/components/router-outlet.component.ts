import { Component } from '@angular/core';

@Component({
    selector: 'router-outlet-container',
    template: `
        <router-outlet></router-outlet>
        <router-outlet name ="sale-o"></router-outlet>
        <router-outlet name ="sale-o-q"></router-outlet>
    `,
    styleUrls: ['']
})
export class RouterOutletComponent {

}
