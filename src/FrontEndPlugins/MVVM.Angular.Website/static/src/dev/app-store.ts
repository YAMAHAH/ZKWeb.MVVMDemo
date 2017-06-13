import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/distinctUntilChanged';
import { ClientDataModel } from '../business_bases/desktop/global_module/models/client-data-model';

export interface State {
    // define your state here
    clientData?: ClientDataModel;
    accessToken?: string;
}

const defaultState: State = {
    // define your initial state here
    clientData: null,
    accessToken: ""
}

const _store = new BehaviorSubject<State>(defaultState);

@Injectable()
export class AppStore {
    private _store = _store;
    changes = this._store
        .asObservable()
        .distinctUntilChanged()

    setState(state: State) {
        this._store.next(state);
    }

    getState(): State {
        return this._store.value;
    }

    purge() {
        this._store.next(defaultState);
    }
}