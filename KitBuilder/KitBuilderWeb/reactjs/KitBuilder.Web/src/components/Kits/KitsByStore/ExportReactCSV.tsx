import React from 'react'
import Button from '@material-ui/core/Button';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';

export const ExportCSV = ({ csvData, fileName, disableExportbutton, worksheetColumnsWidth,excludeColumnsInExport }: any) => {

    const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const fileExtension = '.xlsx';
    
    const exportToCSV = (csvData: any, fileName: any) => {
        excludeColumnsInExport.forEach(function(col:string){
            csvData.forEach(function(v:any){ delete v[col] })
        });

        const ws = XLSX.utils.json_to_sheet(csvData);
        ws['!cols'] = worksheetColumnsWidth;
        const wb = { Sheets: { 'kitsByStore': ws }, SheetNames: ['kitsByStore'] };
        const excelBuffer = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
        const data = new Blob([excelBuffer], { type: fileType });
        FileSaver.saveAs(data, fileName + fileExtension);
    }

    return (
        <Button disabled={disableExportbutton} variant="contained" color="primary" onClick={(e: any) => exportToCSV(csvData, fileName)}>Export</Button>
    )
}