const code128BarcodeLengthLong = 27;
const code128BarcodeLengthStandard = 13;
const trimLeadingZeroesRegExp = new RegExp('^0+');

const ScanCodeProcessor = {
    //return a scan code from a passed in scan code and symbology
    parseScanCode: (scanCode: string, symbology?: string): string => {
        if (!scanCode || scanCode === '') {
            throw new Error('Scan code is null or empty.');
        } else {
            if(!symbology || symbology === '') {
                console.warn('No symbology supplied so unable to determine the type of scan code. Returning scan code without leading zeroes.');
                return scanCode.replace(trimLeadingZeroesRegExp, '');
            }
            if (symbology === 'Code128' || symbology === 'GS1_128' || symbology === 'QR') {
                if(scanCode.length === code128BarcodeLengthLong) {
                    //skip first 2 characters which are 01 in the string and are and indicator for this type of scan code
                    //then take next 13 characters which are the scan code with trimmed leading zeroes.
                    return scanCode.substring(2)
                        .substring(0, 13)
                        .replace(trimLeadingZeroesRegExp, '');
                } else if(scanCode.length === code128BarcodeLengthStandard) {
                    return scanCode.replace(trimLeadingZeroesRegExp, '');
                } else {
                    console.warn('Unexpected scan code length. Returning scan code without leading zeroes.');
                    return scanCode.replace(trimLeadingZeroesRegExp, '');
                }
            } else if(symbology === 'UPCA') {
                let trimmedScanCode = scanCode.replace(trimLeadingZeroesRegExp, '');
                if(trimmedScanCode.startsWith('2') && trimmedScanCode.length === 11) {
                    //return scan code with the last 5 digits replaced as zeroes
                    return trimmedScanCode
                        .substring(0, 6)
                        .padEnd(11, '0');
                } else {
                    return trimmedScanCode;
                }
            } else {
                throw new Error(`Unable to process scan code "${scanCode}". Unhandled symbology "${symbology}".`);
            }
        }
    }
};

export default ScanCodeProcessor;