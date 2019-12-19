export declare enum ScannerState {
    Idle = "Idle",
    Claiming = "Claiming",
    Claimed = "Claimed"
}
export declare class BarcodeScanner {
    private scanHandler;
    scannerState: ScannerState;
    constructor();
    registerHandler(method: (data: string) => void): void;
    receiveScanData(data: string): void;
    claimScanner(): void;
    releaseScanner(): void;
}
