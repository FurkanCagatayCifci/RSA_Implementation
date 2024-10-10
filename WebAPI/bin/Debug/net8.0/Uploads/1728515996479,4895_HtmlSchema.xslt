<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" indent="yes" />

	<!-- Root template -->
	<xsl:template match="/">
		<html>
			<head>
				<title>Library Books</title>
				<style>
					table { width: 50%; border-collapse: collapse; }
					table, th, td { border: 1px solid black; }
					th, td { padding: 8px; text-align: left; }
					th { background-color: #f2f2f2; }
				</style>
			</head>
			<body>
				<h2>Library Books</h2>
				<table>
					<tr>
						<th>Title</th>
						<th>Author</th>
						<th>Year</th>
						<th>Price</th>
					</tr>
					<xsl:for-each select="books/book">
						<tr>
							<td>
								<xsl:value-of select="title" />
							</td>
							<td>
								<xsl:value-of select="author" />
							</td>
							<td>
								<xsl:value-of select="year" />
							</td>
							<td>
								<xsl:value-of select="price" />
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
l:stylesheet