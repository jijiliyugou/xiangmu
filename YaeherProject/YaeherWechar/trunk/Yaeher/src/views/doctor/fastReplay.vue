<template>
    <div class="fastReplay padding-top">
        <router-view/>
		 <mt-header fixed title="快捷回复">
            <a @click="$router.push({path: '/order-detail', query: {id}})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-replay" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div ref="srcollBox" class="content listAbout">
			<div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字查询" />
                    <div @click="getArticleList(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getArticleList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="article-case">
				<li v-for="(item, index) in articleList" :key="index" class="">
			        <a class="articleList">
			        	<!-- <img v-if="item.imageFie !== null" :src="item.imageFie"  alt="" />
                    	<img v-if="item.imageFie === null" src="../../assets/image/article3.jpg" alt="" /> -->
			        	<div class="articleCase">
			        		<!-- <p class="blod">{{item.paperTiltle}}</p> -->
			        		<p @click="sendReplay(item.content)" class="line-textov">{{item.content}}</p>
                            <div class="operation-control">
                                <mt-button @click="detailHref(item.id)" type="primary">修改</mt-button>
                            </div>
			        	</div>
			        </a>
			    </li>
			</ul>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            id: 0,
			keyWord: '',
			skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
			articleList: [],
			loading: true,
			homeTop: 0
        }
    },
    watch:{
        $route(now, old) {
            if (now.name != old.name) {
                this.getArticleList(true)
			}
			
        }
    },
	methods: {
		sendReplay (content1) {
			let content = content1
			let id = this.id
			let childrenDate = {
				content
			}
			this.$emit('listenChildrenEvent', childrenDate)
			this.$router.push({ path: '/order-detail', query: {id}})
		},
		getArticleList (flag) {
			this.loading = true
			if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
			const keyWord = this.keyWord
			const skipCount = this.skipCount
			const maxResultCount = this.maxResultCount
			if(skipCount > this.totalPage) {
				return
			}
            this.instance.quickReplyPageD({
				keyWord,
				skipCount,
				maxResultCount
            })
                .then((response) => {
					if(response.data.result.code===200) {
						setTimeout(() => {
							this.loading = false
							let moreFlag = response.data.result.item.items
							if (!moreFlag || moreFlag.length===0) {
								this.loading = true
							}
							if (flag) this.articleList = []
							let articleList1 = response.data.result.item.items
							this.articleList = this.articleList.concat(articleList1)
							for (let i = 0; i < this.articleList.length; i++) {
								this.articleList[i].content = this.articleList[i].content.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")
							}
							this.skipCount ++
							this.totalPage = response.data.result.item.totalPage
						}, 200);
					} else if (response.data.result.code===204) {
						this.articleList = []
					}
						
                })
                .catch((error) => {
					this.loading = true
                }) 
        },
        detailHref (id1) {
            const id = id1
            this.$router.push({ path: '/add-replay', query: {id}})
        },
		searchKeypress() {
            if (event.keyCode == 13) {
                this.getArticleList(true)
            } 
        },
	},
	mounted () {
		this.getArticleList()
	},
	created () {
        let id1 = this.$route.query.id
        if (id1) {
            this.id = parseInt(id1)
        }
    }
    
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

// body, html {
// 	overflow: hidden;
// }
.fastReplay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: #fff;
    z-index: 2000;
	.listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
	.article-case {
		margin-top: 10px;
		li {
			padding: 15px 0; 
			display: flex;
			.doctor-a {
				display: block;
				width: 100%;
				.doctorInfo {
					img{
						display: block;
						width: 60px;
						height: 60px;
						border-radius: 5px;
					}
					.flexCase {
						padding: 0 10px; 
						p {
							font-size: $font-m;
							color: $color-bfont;
							padding-bottom: 2px;
						}
						.doctor-name {
							font-size: $font-xl;
							color: $color-afont;
						}
						.star-font {
							color: $color-star;
						}
					}
				}  
			}
		}
	}
	.articleList {overflow: hidden; display: flex; position: relative; display: block; width: 100%;}
	.articleList img {float: left; width: 60px; height: 60px; border-radius: 8px;}
	.articleList .articleCase {padding: 0 0 0 10px; display: flex;}
	.articleCase p {font-size: $font-l; color: $color-bfont;}
	.articleCase .blod {font-size: $font-xl; color: $color-afont;}
	.articleCase .line-textov {
        width: 260px; 
        overflow : hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
		-webkit-line-clamp: 3;
		word-break: break-all;
		-webkit-box-orient: vertical;
    }
    .operation-control {
        flex: 1;
        text-align: right;
        .mint-button {
            height: 35px;
            font-size: 14px;
        }

    }
}

</style>