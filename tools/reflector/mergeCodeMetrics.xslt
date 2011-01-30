<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>

  <!-- Merge the Type, Method and Module elements into one Report -->
  <xsl:template match="/">
    <Report>
      <Metric Name="Type">
        <xsl:copy-of select="//Report/Metric/Type[@Name!='&lt;Module&gt;']"/>
      </Metric>
      <Metric Name="Method">
        <xsl:copy-of select="//Report/Metric/Method"/>
      </Metric>
      <Metric Name="Module">
        <xsl:copy-of select="//Report/Metric/Module"/>
      </Metric>
    </Report>
  </xsl:template>
  
</xsl:stylesheet>
