<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	>

	<xsl:output method="xml" indent="yes"/>

	<!-- Root template to match the whole document -->
	<xsl:template match="/">
		<!-- Apply templates to all children of the root -->
		<xsl:apply-templates/>
	</xsl:template>

	<!-- Template to match and transform each book -->
	<xsl:template match="books/book">
		<product>
			<name>
				<xsl:value-of select="title"/>
			</name>
			<creator>
				<xsl:value-of select="author"/>
			</creator>
			<yearPublished>
				<xsl:value-of select="year"/>
			</yearPublished>
			<price>
				<xsl:value-of select="price"/>
			</price>
		</product>
	</xsl:template>

</xsl:stylesheet>
l:stylesheet>