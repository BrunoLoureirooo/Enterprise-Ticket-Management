import { Injectable, signal } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class NavService {
    isOpen = signal(true);

    toggle() {
        this.isOpen.set(!this.isOpen());
    }
}
