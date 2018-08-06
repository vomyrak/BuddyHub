<!-- 
<?xml:stylesheet href="doc.xsl" type="text/xsl"?> 
-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/TR/WD-xsl">
<xsl:script>
<![CDATA[
function isType(node) {
    return node.getAttribute("name").charAt(0) == 'T';
}

function isConstructor(node) {
    var name = node.getAttribute("name");
    return name.charAt(0) == 'M' && name.indexOf("#ctor") >= 0;
}
        
function isMethod(node) {
    var name = node.getAttribute("name");
    return name.charAt(0) == 'M' && name.indexOf("#ctor") < 0;
}

function isProperty(node) {
    var name = node.getAttribute("name");
    return name.charAt(0) == 'P' && name.indexOf("(") < 0;
}

function isIndexer(node) {
    var name = node.getAttribute("name");
    return name.charAt(0) == 'P' && name.indexOf("(") >= 0;
}

function isField(node) {
    return node.getAttribute("name").charAt(0) == 'F';
}

function isEvent(node) {
    return node.getAttribute("name").charAt(0) == 'E';
}

function fullName(node) {
    return node.getAttribute("name").substr(2);
}

function memberName(node, attr) {
    var cref = node.getAttribute(attr);
    var name = cref.substr(2);
    var p = name.indexOf("(");
    if (p == -1) {
        s = shortName(name);
        if (s == "#ctor") s = shortName(name.substr(0, name.length - 6));
        if (cref.charAt(0) == 'M') s = s + "()";
        return s;
    }
    else {
        s = shortName(name.substr(0, p));
        if (s == "#ctor") s = shortName(name.substr(0, p - 6));
        params = name.substr(p + 1, name.indexOf(")") - p - 1).split(",");
        for (i = 0; i < params.length; i++) params[i] = shortName(params[i]);
        if (cref.charAt(0) == 'P') return "this[" + params.join(",") + "]";
        return s + "(" + params.join(",") + ")";
    }
}

function shortName(name) {
    return name.substr(name.lastIndexOf(".") + 1);
}
]]>
</xsl:script>

<xsl:template match="/">
<HTML>
<HEAD>
<TITLE><xsl:value-of select="doc/assembly/name"/></TITLE>
<LINK rel="stylesheet" type="text/css" href="doc.css"/>
</HEAD>
<BODY>
    <xsl:apply-templates select="doc/members/member"/>
</BODY>
</HTML>
</xsl:template>

<xsl:template match="member">
    <xsl:choose>
		<xsl:when expr="isType(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h1><xsl:eval>fullName(this)</xsl:eval></h1></a>
			<xsl:apply-templates select="summary"/>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
		<xsl:when expr="isMethod(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h2><xsl:eval>memberName(this, "name")</xsl:eval> method</h2></a>
			<xsl:apply-templates select="summary"/>
			<xsl:if test="param">
				<h4>Parameters</h4>
				<dl><xsl:apply-templates select="param"/></dl>
			</xsl:if>
			<xsl:apply-templates select="returns"/>
			<xsl:if test="exception">
				<h4>Exceptions</h4>
				<dl><xsl:apply-templates select="exception"/></dl>
			</xsl:if>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
		<xsl:when expr="isConstructor(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h2><xsl:eval>memberName(this, "name")</xsl:eval> constructor</h2></a>
			<xsl:apply-templates select="summary"/>
			<xsl:if test="param">
				<h4>Parameters</h4>
				<dl><xsl:apply-templates select="param"/></dl>
			</xsl:if>
			<xsl:if test="exception">
				<h4>Exceptions</h4>
				<dl><xsl:apply-templates select="exception"/></dl>
			</xsl:if>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
		<xsl:when expr="isProperty(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h2><xsl:eval>memberName(this, "name")</xsl:eval> property</h2></a>
			<xsl:apply-templates select="summary"/>
			<xsl:apply-templates select="value"/>
			<xsl:if test="exception">
				<h4>Exceptions</h4>
				<dl><xsl:apply-templates select="exception"/></dl>
			</xsl:if>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
		<xsl:when expr="isIndexer(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h2><xsl:eval>memberName(this, "name")</xsl:eval> indexer</h2></a>
			<xsl:apply-templates select="summary"/>
			<xsl:if test="param">
				<h4>Parameters</h4>
				<dl><xsl:apply-templates select="param"/></dl>
			</xsl:if>
			<xsl:apply-templates select="value"/>
			<xsl:if test="exception">
				<h4>Exceptions</h4>
				<dl><xsl:apply-templates select="exception"/></dl>
			</xsl:if>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
		<xsl:when expr="isField(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h2><xsl:eval>memberName(this, "name")</xsl:eval> field</h2></a>
			<xsl:apply-templates select="summary"/>
			<xsl:apply-templates select="value"/>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
		<xsl:when expr="isEvent(this)">
            <a><xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute><h2><xsl:eval>memberName(this, "name")</xsl:eval> event</h2></a>
			<xsl:apply-templates select="summary"/>
			<xsl:apply-templates select="remarks"/>
			<xsl:apply-templates select="example"/>
			<xsl:if test="seealso">
				<h4>See Also</h4>
				<xsl:apply-templates select="seealso"/>
			</xsl:if>
		</xsl:when>
	</xsl:choose>
</xsl:template>

<xsl:template match="summary"><p><xsl:apply-templates/></p></xsl:template>

<xsl:template match="param">
    <dt><i><xsl:value-of select="@name"/></i></dt>
	<dd><xsl:apply-templates/></dd>
</xsl:template>

<xsl:template match="value">
    <h4>Value</h4>
	<xsl:apply-templates/>
</xsl:template>

<xsl:template match="returns">
    <h4>Returns</h4>
	<xsl:apply-templates/>
</xsl:template>

<xsl:template match="exception">
    <dt><i><xsl:eval>memberName(this, "cref")</xsl:eval></i></dt>
	<dd><xsl:apply-templates/></dd>
</xsl:template>

<xsl:template match="remarks">
    <h4>Remarks</h4>
	<xsl:apply-templates/>
</xsl:template>

<xsl:template match="example">
    <h4>Example</h4>
	<xsl:apply-templates/>
</xsl:template>

<xsl:template match="seealso">
    <xsl:if test="cref"><a><xsl:attribute name="href">#<xsl:value-of select="@cref"/></xsl:attribute><xsl:eval>memberName(this, "cref")</xsl:eval></a>&#160;</xsl:if>
</xsl:template>

<xsl:template match="text()"><xsl:value-of/></xsl:template>

<xsl:template match="para"><p><xsl:apply-templates/></p></xsl:template>

<xsl:template match="code"><pre><xsl:value-of/></pre></xsl:template>

<xsl:template match="see">
    <xsl:choose>
        <xsl:when test="@langword"><code><xsl:value-of select="@langword"/></code></xsl:when>
        <xsl:when test="@cref"><a><xsl:attribute name="href">#<xsl:value-of select="@cref"/></xsl:attribute><xsl:eval>memberName(this, "cref")</xsl:eval></a></xsl:when>
    </xsl:choose>
</xsl:template>

<xsl:template match="list"><table><xsl:apply-templates/></table></xsl:template>

<xsl:template match="listheader"><tr><xsl:apply-templates/></tr></xsl:template>

<xsl:template match="item"><tr><xsl:apply-templates/></tr></xsl:template>

<xsl:template match="term"><td><xsl:apply-templates/></td></xsl:template>

<xsl:template match="description"><td><xsl:apply-templates/></td></xsl:template>

<xsl:template match="c"><code><xsl:apply-templates/></code></xsl:template>

<xsl:template match="paramref"><i><xsl:value-of select="@name"/></i></xsl:template>

</xsl:stylesheet>