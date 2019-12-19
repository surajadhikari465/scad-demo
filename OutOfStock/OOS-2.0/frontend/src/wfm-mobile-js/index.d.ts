import { BarcodeScanner } from './BarcodeScanner';
declare global {
    interface Window {
        receiveEvent: (id: string, data: string) => void;
    }
}
declare const scanner: BarcodeScanner;
export { scanner as BarcodeScanner };
