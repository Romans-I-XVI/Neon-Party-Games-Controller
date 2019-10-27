	/* Data SHA1: 0bea9ca01c0c9349626ed1c7f58a14acfae3fccb */
	.file	"typemap.jm.inc"

	/* Mapping header */
	.section	.data.jm_typemap,"aw",@progbits
	.type	jm_typemap_header, @object
	.p2align	2
	.global	jm_typemap_header
jm_typemap_header:
	/* version */
	.long	1
	/* entry-count */
	.long	7
	/* entry-length */
	.long	140
	/* value-offset */
	.long	65
	.size	jm_typemap_header, 16

	/* Mapping data */
	.type	jm_typemap, @object
	.global	jm_typemap
jm_typemap:
	.size	jm_typemap, 981
	.include	"typemap.jm.inc"
