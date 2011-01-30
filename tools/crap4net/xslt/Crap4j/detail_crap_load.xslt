<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.w3.org/1999/xhtml" exclude-result-prefixes="xhtml xsl">
<xsl:output method="xml" version="1.0" encoding="UTF-8" doctype-public="-//W3C//DTD XHTML 1.1//EN" doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd" indent="yes"/>

	<xsl:strip-space elements="package className"/>

	<xsl:template match="/crap_result">
   		<html>
			<head>
				<title>CRAP Report Detail for <xsl:value-of select="project"/></title>
				<style type="text/css">
					body {font-family: sans-serif}
					h2 {text-align: center; padding: 0; margin: 0.5em}
					h4 {text-align: center; padding: 0; margin: 0.5em}
					table {border: 1px solid; border-spacing: 0; margin: 1em auto 1em auto}
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
				<h2>CRAP Report Detail</h2><h4>(Sorted by Crap Load)</h4><h3 style="text-align: center;">Project: <xsl:value-of select="project"/></h3>
				<center><a href="index.html">Overview Page</a> |
				<a href="detail_crap.html">CRAP</a> |
				<a href="detail_complexity.html">Complexity</a> |
				<a href="detail_coverage.html">Coverage</a></center>
 				<xsl:apply-templates select="methods"/>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="methods">
		<table border="1">
			<tr class="header">
				<th class="method">Method</th>
				  <th class="complexity">Complexity</th>
			  	<th class="coverage">Coverage</th>
			  	<th class="crap">CRAP</th>
			  	<th class="crap">CRAP Load</th>
			</tr>
			<xsl:for-each select="method">
				<xsl:sort data-type="number" order="descending" select="crapLoad"/>
				<xsl:apply-templates select="."/>
			</xsl:for-each>
		</table>
	</xsl:template>

	<xsl:template match="method">
		<tr>
			<td class="method">
				<p class="method"><xsl:value-of select="fullMethod"/></p>
				<xsl:choose>
					<xsl:when test="package">
						<p class="class"><xsl:value-of select="package"/><xsl:text>.</xsl:text><xsl:value-of select="className"/></p>
					</xsl:when>
					<xsl:otherwise>
						<p class="class"><xsl:value-of select="className"/></p>
					</xsl:otherwise>
				</xsl:choose>
			</td>
			<td class="complexity"><xsl:value-of select="complexity"/></td>
			<td class="coverage"><xsl:value-of select="coverage"/>%</td>
			<td class="crap"><xsl:value-of select="crap"/></td>
			<td class="crap"><xsl:value-of select="crapLoad"/></td>			
		</tr>
	</xsl:template>
	
</xsl:stylesheet>
