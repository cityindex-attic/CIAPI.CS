<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.w3.org/1999/xhtml" exclude-result-prefixes="xhtml xsl">
<xsl:output method="xml" version="1.0" encoding="UTF-8" doctype-public="-//W3C//DTD XHTML 1.1//EN" doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd" indent="yes"/>

	<xsl:strip-space elements="package className"/>

	<xsl:template match="/crap_result">
   		<html>
			<head>
				<title>CRAP Report for <xsl:value-of select="project"/></title>
				<style type="text/css">
					body {font-family: sans-serif}
					h2 {text-align: center; padding: 0; margin: 0.5em}
					h4 {text-align: center; padding: 0; margin: 0.5em}
					table {border: 0px ; border-spacing: 0; margin: 1em auto 1em auto}
					tr.diagnostic:hover {background-color: #e0e0e0}
					th {background-color: #ccccff; border: 1px solid; margin: 0; padding: 5px}
					td {border: 1px solid; margin: 0; padding: 5px}
					.show {color: blue; text-decoration: underline; cursor: pointer}
					p.class {margin: 0; padding: 0; font-size: smaller}
					p.method {margin: 0; padding: 0}
					th.stat {width: 50%; text-align: left; background-color: #e0e0e0}
					td.value {width: 50%; text-align: right}
					td.complexity {text-align: right; width: 10em}
					td.coverage {text-align: right; width: 10em}
					td.crap {text-align: right; width: 10em}
				</style>
				<script type="text/javascript">
					function show(id) {
						document.getElementById(id).style.display='';
						document.getElementById("show-" + id).style.display="none";
					}
				</script>
			</head>
			<body>
				<h2>CRAP Report</h2>
				<h4>Project: <xsl:value-of select="project"/></h4>
				<p style="text-align:center">Generated at <xsl:value-of select="timestamp"/></p>
 				<xsl:apply-templates select="stats"/>
				<center>Method Detail: Sorted by: <a href="detail_crap_load.html">CRAP Load</a> |
				<a href="detail_crap.html">CRAP</a> |
				<a href="detail_complexity.html">Complexity</a> |
				<a href="detail_coverage.html">Coverage</a></center>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="stats">
	<center>
		<table><tr colspan="2"><td class="stat"><center><h2>Percentage of CRAPpy Methods</h2><img src="crapBar.png" /></center></td></tr>
		<tr>
		<td>
		<table>
			<tr>
				<th colspan="2" class="header">Summary</th>
			</tr>
			<!--
			<tr>
				<th class="stat">Total CRAP</th>
			  	<td class="value"><xsl:value-of select="totalCrap"/></td>
			</tr>			
			<tr>
				<th class="stat">Median</th>
			  	<td class="value"><xsl:value-of select="median"/></td>
			</tr>
			<tr>
				<th class="stat">Average</th>
			  	<td class="value"><xsl:value-of select="average"/></td>
			</tr>
			<tr>
				<th class="stat">Standard Deviation</th>
			  	<td class="value"><xsl:value-of select="stdDev"/></td>
			</tr>
			-->
			<tr>
				<th class="stat"><b>Percentage of CRAPpy Methods</b></th>
			  	<td class="value"><b><xsl:value-of select="crapMethodPercent"/>%</b></td>
			</tr>
			<tr>
				<th class="stat"><b>CRAP Load</b></th>
			  	<td class="value"><b><xsl:value-of select="crapLoad"/></b></td>
			</tr>
			<tr>
				<th class="stat">Total Method Count</th>
			  	<td class="value"><xsl:value-of select="methodCount"/></td>
			</tr>
			<tr>
				<th class="stat">CRAPpy Method Count (CRAP &gt;<xsl:value-of select="crapThreshold"/>)</th>
			  	<td class="value"><xsl:value-of select="crapMethodCount"/></td>
			</tr>
			
			
		</table>
		</td></tr></table>
		</center>
		<style>
    #vertgraph {                    
        width: 677px; 
        height: 207px; 
        position: relative; 	
        margin: 0 auto; 
        background: url("g_backbar.gif") no-repeat; 
    }
    #vertgraph ul { 
        width: 677px; 
        height: 207px; 
        margin: 0; 
        padding: 0; 
    }
    #vertgraph ul li {  
        position: absolute; 
        width: 28px; 
        height: 160px; 
        bottom: 34px; 
        padding: 0 !important; 
        margin: 0 !important; 
        background: url("g_colorbar3.jpg") no-repeat !important;
        text-align: center; 
        font-weight: bold; 
        color: white; 
        line-height: 2.5em;
    }

    #vertgraph li.one { left: 24px; background-position: 0px bottom !important; }
    #vertgraph li.two { left: 101px; background-position: -28px bottom !important; }
    #vertgraph li.four { left: 176px; background-position: -56px bottom !important; }
    #vertgraph li.eight { left: 251px; background-position: -84px bottom !important; }
    #vertgraph li.sixteen { left: 327px; background-position: -112px bottom !important; }
    #vertgraph li.thirtytwo { left: 402px; background-position: -140px bottom !important; }
    #vertgraph li.sixtyfour { left: 477px; background-position: -168px bottom !important; }
    #vertgraph li.one28 { left: 552px; background-position: -196px bottom !important; }
    #vertgraph li.two56 { left: 627px; background-position: -224px bottom !important; }
</style>

<div style="position: relative;  margin: 0 auto; text-align: center;">
<h3>Method CRAP Distribution</h3>
<div id="vertgraph">
    <ul>
    <xsl:for-each select="histogram/hist">
        <li class="{place}" style="height: {height};"><xsl:value-of select="value"/></li>
    </xsl:for-each>    
    </ul>
</div>
<p>  </p>
</div>
		

	</xsl:template>


</xsl:stylesheet>
