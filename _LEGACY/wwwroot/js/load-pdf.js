
         // Loaded via <script> tag, create shortcut to access PDF.js exports.
        var pdfjsLib = window['pdfjs-dist/build/pdf'];

        // The workerSrc property shall be specified.
        pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';

        // Asynchronous download of PDF
        var loadingTask = pdfjsLib.getDocument(url);
        loadingTask.promise.then(function(pdf) {
        var ctx = document.createElement('canvas').getContext('2d', { alpha: false });
          console.log('PDF loaded');

          // Fetch the first page
          var pageNumber = 1;
          pdf.getPage(pageNumber).then(function(page) {
            console.log('Page loaded');
            var scale = 1.5;
            var viewport = page.getViewport(scale);
            
            // Prepare canvas using PDF page dimensions
            var canvas = document.getElementById('pdf-image');
            var context = canvas.getContext('2d');
            canvas.height = viewport.height;
            canvas.width = viewport.width;
            
            // Render PDF page into canvas context
            var renderContext = {
              canvasContext: context,
              viewport: viewport
            };
            var renderTask = page.render(renderContext);
            renderTask.then(function () {
              console.log('Page rendered');
            });
                page.getTextContent({ normalizeWhitespace: true }).then(function (textContent) {
                textContent.items.forEach(function (textItem) {
                var tx = pdfjsLib.Util.transform(
                  pdfjsLib.Util.transform(viewport.transform, textItem.transform),
                  [1, 0, 0, -1, 0, 0]
                );
        
                var style = textContent.styles[textItem.fontName];
                var pageContainer = document.getElementById('text-overlay');
                pageContainer.classList.add('page-container');
                pageContainer.style.width = viewport.width + 'px';
                pageContainer.style.height = viewport.height + 'px';
                // adjust for font ascent/descent
                var fontSize = Math.sqrt((tx[2] * tx[2]) + (tx[3] * tx[3]));
        
                if (style.ascent) {
                  tx[5] -= fontSize * style.ascent;
                } else if (style.descent) {
                  tx[5] -= fontSize * (1 + style.descent);
                } else {
                  tx[5] -= fontSize / 2;
                }
        
                // adjust for rendered width
                if (textItem.width > 0) {
                  ctx.font = tx[0] + 'px ' + style.fontFamily;
        
                  var width = ctx.measureText(textItem.str).width;
        
                  if (width > 0) {
                    //tx[0] *= (textItem.width * viewport.scale) / width;
                    tx[0] = (textItem.width * viewport.scale) / width;
                  }
                }
            /*
                var item = document.createElement('span');
                var S = textItem.str;
                var replaceNome = S.replace(/_ALUNO_/i, '<?php echo $aluno->getNomeAluno(); ?>');
                var replaceRA= replaceNome.replace(/_RA_/i, '<?php echo $aluno->getRA(); ?>');
                var replacePeriodo= replaceRA.replace(/_PERIODO_/i, '<?php echo $aluno->getPeriodo(); ?>');
                var replaceTurno= replacePeriodo.replace(/_TURNO_/i, '<?php echo $aluno->getTurno(); ?>');
                var replaceDDDTel= replaceTurno.replace(/_DDDT_/i, '<?php echo $aluno->getDDDTel(); ?>');
                var replaceTel= replaceDDDTel.replace(/_TELEFONE_/i, '<?php echo $aluno->getTelefone(); ?>');
                var replaceDDDCel= replaceTel.replace(/_DDDC_/i, '<?php echo $aluno->getDDDCel(); ?>');
                var replaceCel = replaceDDDCel.replace(/_CELULAR_/i, '<?php echo $aluno->getCelular(); ?>');
                var replaceEmail = replaceCel.replace(/_EMAIL_/i, '<?php echo $aluno->getEmail(); ?>');
                var replaceEmpresa= replaceEmail.replace(/_EMPRESA_/i, '<?php echo $aluno->getNomeEmpresa(); ?>');
                
                textItem.str=replaceEmpresa;
                item.textContent = textItem.str;
                item.style.fontFamily = style.fontFamily;
                item.style.fontSize = fontSize + 'px';
                item.style.transform = 'scaleX(' + tx[0] + ')';
                item.style.left = tx[4] + 'px';
                item.style.top = tx[5] + 'px';
                pageContainer.appendChild(item);*/
                });
            });
          });
        
        
        }, function (reason) {
          // PDF loading error
          console.error(reason);
        });
        
        //PEGANDO PDF SEM TEXTO DE PREENCHIMENTO
        
        /*
        // Asynchronous download of PDF
        var loadingTask = pdfjsLib.getDocument(url2);
        loadingTask.promise.then(function(pdf) {
        var ctx = document.createElement('canvas').getContext('2d', { alpha: false });
          console.log('PDF loaded');

          // Fetch the first page
          var pageNumber = 1;
          pdf.getPage(pageNumber).then(function(page) {
            console.log('Page loaded');
            var scale = 1.5;
            var viewport = page.getViewport(scale);
            
            // Prepare canvas using PDF page dimensions
            var canvas = document.getElementById('pdf-stp');
            var context = canvas.getContext('2d');
            canvas.height = viewport.height;
            canvas.width = viewport.width;
            
            // Render PDF page into canvas context
            var renderContext = {
              canvasContext: context,
              viewport: viewport
            };
            var renderTask = page.render(renderContext);
            renderTask.then(function () {
              console.log('Page rendered');
            });
                page.getTextContent({ normalizeWhitespace: true }).then(function (textContent) {
                textContent.items.forEach(function (textItem) {
                var tx = pdfjsLib.Util.transform(
                  pdfjsLib.Util.transform(viewport.transform, textItem.transform),
                  [1, 0, 0, -1, 0, 0]
                );
        
                var style = textContent.styles[textItem.fontName];
                var pageContainer = document.getElementById('');
                pageContainer.classList.add('page-container');
                pageContainer.style.width = viewport.width + 'px';
                pageContainer.style.height = viewport.height + 'px';
                // adjust for font ascent/descent
                var fontSize = Math.sqrt((tx[2] * tx[2]) + (tx[3] * tx[3]));
        
                if (style.ascent) {
                  tx[5] -= fontSize * style.ascent;
                } else if (style.descent) {
                  tx[5] -= fontSize * (1 + style.descent);
                } else {
                  tx[5] -= fontSize / 2;
                }
        
                // adjust for rendered width
                if (textItem.width > 0) {
                  ctx.font = tx[0] + 'px ' + style.fontFamily;
        
                  var width = ctx.measureText(textItem.str).width;
        
                  if (width > 0) {
                    //tx[0] *= (textItem.width * viewport.scale) / width;
                    tx[0] = (textItem.width * viewport.scale) / width;
                  }
                }
        
                });
            });
          });
        
        
        }, function (reason) {
          // PDF loading error
          console.error(reason);
        });
       }*/