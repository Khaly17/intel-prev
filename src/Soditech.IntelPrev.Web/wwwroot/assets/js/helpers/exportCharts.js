(()=>{
    
    function printCharts(pageTitle, divId) {
        let elementToPrint = document.querySelector('#' + divId);
        if (elementToPrint) {
            let printWindow = window.open('', '_blank');
            printWindow.document.open();
            printWindow.document.write('<html lang="fr"><head><title>'+ pageTitle +' </title></head><body>');
            printWindow.document.write(elementToPrint.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
            printWindow.close();
        }
    }

    function downloadCharts(divId, fileName) {
        let elementToPrint = document.querySelector('#' + divId);
        if (elementToPrint) {
            let canvas = document.createElement('canvas');
            canvas.width = elementToPrint.offsetWidth; //offsetWidth scrollWidth
            canvas.height = elementToPrint.offsetHeight; //offsetHeight scrollHeight
            let context = canvas.getContext('2d');
            context.scale(1, 1);
            html2canvas(elementToPrint, {
                // canvas: canvas,
                scrollX: 0,
                scrollY: -window.scrollY,
            }).then((canvas) => {
                
                let pdf = new jspdf.jsPDF();
                // Add the image in PDF format
                pdf.addImage(canvas, 'PNG', 0, 0, pdf.internal.pageSize.width, pdf.internal.pageSize.height);

                // Save the PDF with specified name
                pdf.save(fileName + '.pdf');
            });
        }
    }

    window.downloadCharts = downloadCharts;
    window.printCharts = printCharts;
    
    
})(jQuery);
