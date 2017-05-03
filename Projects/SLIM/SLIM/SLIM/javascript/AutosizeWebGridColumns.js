function setColWidth(col, rows) {
    var length =0;
	var cell;
	var colel;
	if(col)
    {
	    colel = document.getElementById(col.Id);
        if (col.getVisible()==true)
	    {
		    length = colel.firstChild.offsetWidth;		
			
		    for(var i = 0; i < rows.length; i++)
		    {
			    cell = rows.getRow(i).getCell(col.Index);
			    if(cell.Element.firstChild.offsetWidth > length)
			    {
				    length = cell.Element.firstChild.offsetWidth ;
			    }					
		    }						
				
		    col.setWidth(length);			
	
	    }					
    }
}

function setColWidths(gridName){
    
	var _grid=igtbl_getGridById(gridName);
	var rows = _grid.Rows;
	var col;
	var colel;
	
	for (var iCount = 0; iCount < _grid.Bands[0].Columns.length; iCount++) { 
        col=_grid.Bands[0].Columns[iCount];
        setColWidth(col, rows);
	}
}

