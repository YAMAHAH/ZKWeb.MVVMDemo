import { Routes } from '@angular/router';
import { AdminContainerComponent } from '../admin_base_module/components/admin-container.component';
import { AdminIndexComponent } from './components/admin-index.component';
import { AdminAboutWebsiteComponent } from './components/admin-about-website.component';
import { AdminAboutMeComponent } from './components/admin-about-me.component';
import { AdminLoginComponent } from './components/admin-login.component';
import { AuthGuard } from '@auth_module/auth/auth-guard';
import { UserTypes } from '@generated_module/privileges/user-types';

export const adminRoutesConfig: Routes = [
    {
        path: "",
        component: AdminContainerComponent,
        canActivate: [AuthGuard],
        data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } },
        children:
        [
            {
                path: '', component: AdminIndexComponent, pathMatch: "full"
            },
            {
                path: 'about_website', component: AdminAboutWebsiteComponent,
                //  canActivate: [AuthGuard],
                // data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } }
            },
            {
                path: 'about_me', component: AdminAboutMeComponent,
                // canActivate: [AuthGuard],
                // data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } }
            },
            { path: 'tenants', loadChildren: '../admin_tenants_module/admin_tenants.module#AdminTenantsModule' },
            { path: 'users', loadChildren: '../admin_users_module/admin_users.module#AdminUsersModule' },
            { path: 'roles', loadChildren: '../admin_roles_module/admin_roles.module#AdminRolesModule' },
            { path: 'settings', loadChildren: '../admin_settings_module/admin_settings.module#AdminSettingsModule' },
            { path: 'scheduled_tasks', loadChildren: '../admin_scheduled_tasks_module/admin_scheduled_tasks.module#AdminScheduledTasksModule' },
            { path: 'example_datas', loadChildren: '../admin_example_datas_module/admin_example_datas.module#AdminExampleDatasModule' }

        ]
    },
    { path: 'login', component: AdminLoginComponent }
];