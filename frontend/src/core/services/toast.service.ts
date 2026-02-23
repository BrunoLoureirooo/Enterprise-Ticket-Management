import { Injectable } from '@angular/core';
import notify from 'devextreme/ui/notify';

@Injectable({
    providedIn: 'root',
})
export class ToastService {

    static readonly DEFAULT_DISPLAY_TIME = 2000;

    success(message: string, displayTime: number = ToastService.DEFAULT_DISPLAY_TIME): void {
        this.notify(message, 'success', displayTime);
    }

    error(message: string, displayTime: number = ToastService.DEFAULT_DISPLAY_TIME): void {
        this.notify(message, 'error', displayTime);
    }

    info(message: string, displayTime: number = ToastService.DEFAULT_DISPLAY_TIME): void {
        this.notify(message, 'info', displayTime);
    }

    warning(message: string, displayTime: number = ToastService.DEFAULT_DISPLAY_TIME): void {
        this.notify(message, 'warning', displayTime);
    }

    notify(message: string, type: string = 'info', displayTime: number = ToastService.DEFAULT_DISPLAY_TIME): void {
        notify(
            {
                message,
                type,
                displayTime,
                width: 'auto',
                position: {
                    at: 'top right',
                    my: 'top right',
                    offset: '-10 10',
                },
            },
            type,
            displayTime
        );
    }
}
